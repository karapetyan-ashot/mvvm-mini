using System;
using System.Collections.Generic;
using System.Linq;

using EasySoftware.MvvmMini.Core;

using Unity;
using Unity.Resolution;

namespace EasySoftware.MvvmMini
{
   public class ViewModelFactoryBase : IViewModelFactory
   {
      private readonly Dictionary<Type, Type> _viewModelViewMapping;

      public ViewModelFactoryBase(IUnityContainer unityContainer)
      {
         this.Container = unityContainer ?? throw new ArgumentNullException(nameof(unityContainer));

         this.Container.RegisterType<IViewAdapter, ViewAdapter>();

         this._viewModelViewMapping = new Dictionary<Type, Type>();
      }

      public IUnityContainer Container { get; }

      public void RegisterViewModelWithView<TViewModelFrom, TViewModelTo, TView>() where TViewModelTo : TViewModelFrom, IViewModel
      {
         Type viewModelFromType = typeof(TViewModelFrom);
         if (this._viewModelViewMapping.ContainsKey(viewModelFromType))
            throw new Exception($"Duplicate registration ({viewModelFromType})");
         this._viewModelViewMapping.Add(viewModelFromType, typeof(TView));

         this.Container.RegisterType<TViewModelFrom, TViewModelTo>();
         this.Container.RegisterType<TView>();
      }

      public TViewModel ResolveViewModel<TViewModel>(params (string name, object value)[] constructorParameters) where TViewModel : IViewModel
      {
         Type viewModelType = typeof(TViewModel);
         return (TViewModel)ResolveViewModel(viewModelType, constructorParameters);
      }

      public object ResolveViewModel(Type viewModelType, params (string name, object value)[] constructorParameters)
      {
         if (!this._viewModelViewMapping.ContainsKey(viewModelType))
            throw new Exception($"{viewModelType} is not registered");

         Type viewType = this._viewModelViewMapping[viewModelType];

         var view = this.Container.Resolve(viewType);

         IViewAdapter viewAdapter = this.Container.Resolve<IViewAdapter>(
            new ParameterOverride("view", view));

         var overrides = constructorParameters.Select(x => new ParameterOverride(x.name, x.value)).ToList();
         overrides.Add(new ParameterOverride("viewAdapter", viewAdapter));

         var viewModel = this.Container.Resolve(viewModelType, overrides.ToArray());

         return viewModel;
      }
   }
}