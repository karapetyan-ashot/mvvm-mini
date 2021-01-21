using System;
using Unity;

namespace EasySoftware.MvvmMini.Core
{
	public interface IViewModelFactory
	{
		/// <summary>
		/// IUnityContainer to register and resolve all other services, types except ViewModels.
		/// </summary>
		IUnityContainer Container { get; }

		/// <summary>
		/// Registers in IoC container ViewModel interface, implementation and View.
		/// </summary>
		/// <typeparam name="TViewModelFrom">ViewModel interface</typeparam>
		/// <typeparam name="TViewModelTo">ViewModel implementation</typeparam>
		/// <typeparam name="TView">View</typeparam>
		void RegisterViewModelWithView<TViewModelFrom, TViewModelTo, TView>()
		   where TViewModelTo : TViewModelFrom, IViewModel;

		/// <summary>
		/// Creates and returns requested ViewModel
		/// </summary>
		/// <typeparam name="TViewModel">ViewModel interface</typeparam>
		/// <param name="constructorParameters">View model constructor parameters</param>
		/// <returns>Created view model</returns>
		TViewModel ResolveViewModel<TViewModel>(params (string name, object value)[] constructorParameters)
			where TViewModel : IViewModel;

		/// <summary>
		/// Creates and returns requested ViewModel
		/// </summary>
		/// <typeparam name="viewModelType">ViewModel interface</typeparam>
		/// <param name="constructorParameters">View model constructor parameters</param>
		/// <returns>Created view model</returns>
		object ResolveViewModel(Type viewModelType, params (string name, object value)[] constructorParameters);
	}
}