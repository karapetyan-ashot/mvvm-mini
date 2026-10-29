using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using EasySoftware.MvvmMini.Core;

using Microsoft.Extensions.DependencyInjection;

namespace EasySoftware.MvvmMini
{
    /// <summary>
    /// Extension methods that register MvvmMini with <see cref="IServiceCollection"/> and create view models with their views.
    /// </summary>
    public static class AppInstaller
    {
        /// <summary>
        /// Registers the MvvmMini services: the view model to view mapping and a singleton <see cref="IEventAggregator"/>.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="mappingRegistrator">An action that registers each view model interface with its view model and view.</param>
        /// <returns>The same service collection, so calls can be chained.</returns>
        /// <example>
        /// <code>
        /// services.AddMvvmMini(mapper =>
        /// {
        ///     mapper.RegisterViewModelWithView&lt;IShellViewModel, ShellViewModel, ShellView&gt;();
        /// });
        /// </code>
        /// </example>
        public static IServiceCollection AddMvvmMini(this IServiceCollection services, Action<IViewModelViewMapping> mappingRegistrator)
        {
            services.AddSingleton(sp =>
            {
                IViewModelViewMapping repo = new ViewModelViewMapping();
                mappingRegistrator.Invoke(repo);
                return repo;
            });
            services.AddSingleton<IEventAggregator, EventAggregator>();

            return services;
        }

        /// <summary>
        /// Creates the view model registered for <typeparamref name="TIViewModel"/> together with its view.
        /// </summary>
        /// <typeparam name="TIViewModel">The view model interface that was registered with <see cref="IViewModelViewMapping.RegisterViewModelWithView{TIViewModel, TViewModel, TView}"/>.</typeparam>
        /// <param name="serviceProvider">The service provider used to resolve the constructor dependencies of the view and the view model.</param>
        /// <param name="openArgs">Extra constructor arguments for the view model that are not resolved from the service provider.</param>
        /// <returns>A new view model instance whose view is already created and bound to it.</returns>
        /// <exception cref="Exception">The view model interface is not registered.</exception>
        public static TIViewModel GetViewModel<TIViewModel>(this IServiceProvider serviceProvider, params object[] openArgs)
        {
            Type viewModelInterfaceType = typeof(TIViewModel);
            var viewModel = serviceProvider.GetViewModel(viewModelInterfaceType, openArgs);
            return (TIViewModel)viewModel;
        }

        /// <summary>
        /// Creates the view model registered for the given view model interface together with its view.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve the constructor dependencies of the view and the view model.</param>
        /// <param name="viewModelInterfaceType">The view model interface that was registered with <see cref="IViewModelViewMapping.RegisterViewModelWithView{TIViewModel, TViewModel, TView}"/>.</param>
        /// <param name="openArgs">Extra constructor arguments for the view model that are not resolved from the service provider.</param>
        /// <returns>A new view model instance whose view is already created and bound to it.</returns>
        /// <exception cref="Exception">The view model interface is not registered.</exception>
        public static IViewModel GetViewModel(this IServiceProvider serviceProvider, Type viewModelInterfaceType, params object[] openArgs)
        {
            var vmvMaiping = (IIViewModelViewMappingReader)serviceProvider.GetRequiredService<IViewModelViewMapping>();

            var viewModelType = vmvMaiping.GetViewModel(viewModelInterfaceType);
            var viewType = vmvMaiping.GetView(viewModelInterfaceType);

            var view = (FrameworkElement)ActivatorUtilities.CreateInstance(serviceProvider, viewType);
            var viewAdapter = new ViewAdapter(view);

            openArgs = openArgs.Concat(new[] { viewAdapter }).ToArray();
            return (IViewModel)ActivatorUtilities.CreateInstance(serviceProvider, viewModelType, openArgs);
        }
    }

    /// <summary>
    /// Registers which view model and view belong to a view model interface.
    /// </summary>
    public interface IViewModelViewMapping
    {
        /// <summary>
        /// Registers a view model implementation and its view for a view model interface.
        /// </summary>
        /// <typeparam name="TIViewModel">The view model interface that callers request.</typeparam>
        /// <typeparam name="TViewModel">The view model class that implements <typeparamref name="TIViewModel"/>.</typeparam>
        /// <typeparam name="TView">The view class, usually a <see cref="Window"/> or a <see cref="System.Windows.Controls.UserControl"/>.</typeparam>
        /// <exception cref="Exception"><typeparamref name="TIViewModel"/> is already registered.</exception>
        void RegisterViewModelWithView<TIViewModel, TViewModel, TView>()
            where TIViewModel : IViewModel
            where TViewModel : TIViewModel, IViewModel
            where TView : FrameworkElement;
    }

    internal interface IIViewModelViewMappingReader : IViewModelViewMapping
    {
        Type GetViewModel(Type viewModelInterfaceType);
        Type GetView(Type viewModelInterfaceType);
    }

    internal class ViewModelViewMapping : IIViewModelViewMappingReader
    {
        private Dictionary<Type, (Type ViewModelType, Type ViewType)> _viewModelViewMapping = new Dictionary<Type, (Type ViewModelType, Type ViewType)>();

        public void RegisterViewModelWithView<TIViewModel, TViewModel, TView>()
            where TIViewModel : IViewModel
            where TViewModel : TIViewModel, IViewModel
            where TView : FrameworkElement
        {
            var vmInterfaceType = typeof(TIViewModel);
            if (_viewModelViewMapping.ContainsKey(vmInterfaceType))
                throw new Exception($"Duplicate registration of [{vmInterfaceType.Name}] type");

            _viewModelViewMapping.Add(vmInterfaceType, (typeof(TViewModel), typeof(TView)));
        }

        public Type GetViewModel(Type viewModelInterfaceType)
        {
            if (!_viewModelViewMapping.ContainsKey(viewModelInterfaceType))
                throw new Exception($"ViewModel [{viewModelInterfaceType.Name}] is not registered");

            return _viewModelViewMapping[viewModelInterfaceType].ViewModelType;
        }

        public Type GetView(Type viewModelInterfaceType)
        {
            if (!_viewModelViewMapping.ContainsKey(viewModelInterfaceType))
                throw new Exception($"ViewModel [{viewModelInterfaceType.Name}] is not registered");

            return _viewModelViewMapping[viewModelInterfaceType].ViewType;
        }
    }
}