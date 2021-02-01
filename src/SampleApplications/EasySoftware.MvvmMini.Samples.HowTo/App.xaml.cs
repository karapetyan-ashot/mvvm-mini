using System;
using System.Windows;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.HowTo.Services;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoDialog;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoHandleWindowClosing;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoHandleWindowLoading;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoWindow;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoWindowWithParams;
using Unity;

namespace EasySoftware.MvvmMini.Samples.HowTo
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			IViewModelFactory viewModelFactory = CreateAndConfigureFactory();

			IShellViewModel shellViewModel = viewModelFactory.ResolveViewModel<IShellViewModel>();
			shellViewModel.Closed += (s, ea) => this.Shutdown();
			shellViewModel.Show();
		}

		private IViewModelFactory CreateAndConfigureFactory()
		{
			IViewModelFactory viewModelFactory = new ViewModelFactory();

			// register viewmodels with views
			viewModelFactory.RegisterViewModelWithView<IShellViewModel, ShellViewModel, ShellView>();
			viewModelFactory.RegisterViewModelWithView<IDemoWindowViewModel, DemoWindowViewModel, DemoWindowView>();
			viewModelFactory.RegisterViewModelWithView<IDemoWindowWithParamsViewModel, DemoWindowWithParamsViewModel, DemoWindowWithParamsView>();
			viewModelFactory.RegisterViewModelWithView<IDemoDialogViewModel, DemoDialogViewModel, DemoDialogView>();
			viewModelFactory.RegisterViewModelWithView<IDemoHandleWindowClosingViewModel, DemoHandleWindowClosingViewModel, DemoHandleWindowClosingView>();
			viewModelFactory.RegisterViewModelWithView<IDemoHandleWindowLoadingViewModel, DemoHandleWindowLoadingViewModel, DemoHandleWindowLoadingView>();

			// register other services in UnityContainer
			viewModelFactory.Container.RegisterSingleton<IDateTimeService, DateTimeService>();

			return viewModelFactory;
		}
	}
}