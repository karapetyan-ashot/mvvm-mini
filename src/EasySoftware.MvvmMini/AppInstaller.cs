using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using EasySoftware.MvvmMini.Core;

using Microsoft.Extensions.DependencyInjection;

namespace EasySoftware.MvvmMini
{
    public static class AppInstaller
    {
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

        public static TIViewModel GetViewModel<TIViewModel>(this IServiceProvider serviceProvider, params object[] openArgs)
        {
            var vmvMaiping = (IIViewModelViewMappingReader)serviceProvider.GetRequiredService<IViewModelViewMapping>();

            Type viewModelInterfaceType = typeof(TIViewModel);
            var viewModelType = vmvMaiping.GetViewModel(viewModelInterfaceType);
            var viewType = vmvMaiping.GetView(viewModelInterfaceType);

            var view = (FrameworkElement)ActivatorUtilities.CreateInstance(serviceProvider, viewType);
            var viewAdapter = new ViewAdapter(view);

            openArgs = openArgs.Concat(new[] { viewAdapter }).ToArray();
            return (TIViewModel)ActivatorUtilities.CreateInstance(serviceProvider, viewModelType, openArgs);
        }
    }

    public interface IViewModelViewMapping
    {
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