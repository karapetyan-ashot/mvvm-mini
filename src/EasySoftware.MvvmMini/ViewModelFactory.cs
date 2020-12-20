using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasySoftware.MvvmMini.Core;
using Unity;
using Unity.Resolution;

namespace EasySoftware.MvvmMini
{
   public class ViewModelFactory : IViewModelFactory
   {
      private readonly Dictionary<Type, Type> _viewModelViewMapping;
      private readonly IUnityContainer _unityContainer;

      public ViewModelFactory(IUnityContainer unityContainer)
      {
         this._viewModelViewMapping = new Dictionary<Type, Type>();
         this._unityContainer = unityContainer;

         this._unityContainer.RegisterInstance<IViewModelFactory>(this);
         this._unityContainer.RegisterType<IViewAdapter, ViewAdapter>();
      }

      public void RegisterViewModelWithView<TViewModelFrom, TViewModelTo, TView>() where TViewModelTo : TViewModelFrom, IViewModel
      {
         Type viewModelFromType = typeof(TViewModelFrom);
         if(this._viewModelViewMapping.ContainsKey(viewModelFromType))
            throw new Exception($"Duplicate registration ({viewModelFromType})");
         this._viewModelViewMapping.Add(viewModelFromType, typeof(TView));

         this._unityContainer.RegisterType<TViewModelFrom, TViewModelTo>();
         this._unityContainer.RegisterType<TView>();
      }

      public TViewModel ResolveViewModel<TViewModel>(params KeyValuePair<string, object>[] constructorParameters)
      {
         Type viewModelType = typeof(TViewModel);
         if(!this._viewModelViewMapping.ContainsKey(viewModelType))
            throw new Exception($"{viewModelType} is not registered");

         Type viewType = this._viewModelViewMapping[viewModelType];

         var view = this._unityContainer.Resolve(viewType);

         IViewAdapter viewAdapter = this._unityContainer.Resolve<IViewAdapter>(
            new ParameterOverride("view", view));

         var overrides = constructorParameters.Select(x => new ParameterOverride(x.Key, x.Value)).ToList();
         overrides.Add( new ParameterOverride("viewAdapter", viewAdapter));

         var viewModel = this._unityContainer.Resolve<TViewModel>(overrides.ToArray());

         return viewModel;
      }
   }
}
