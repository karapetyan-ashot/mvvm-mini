using System;
using System.Windows;

using EasySoftware.MvvmMini.Samples.HowTo.Services;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoDialog;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoHandleWindowClosing;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoHandleWindowLoading;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoWindow;
using EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoWindowWithParams;

using Microsoft.Extensions.DependencyInjection;

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

			IServiceCollection services = new ServiceCollection();
			services.AddSingleton<IDateTimeService, DateTimeService>();

			services.AddMvvmMini(mapper =>
			{
				mapper.RegisterViewModelWithView<IShellViewModel, ShellViewModel, ShellView>();
				mapper.RegisterViewModelWithView<IDemoWindowViewModel, DemoWindowViewModel, DemoWindowView>();
				mapper.RegisterViewModelWithView<IDemoWindowWithParamsViewModel, DemoWindowWithParamsViewModel, DemoWindowWithParamsView>();
				mapper.RegisterViewModelWithView<IDemoDialogViewModel, DemoDialogViewModel, DemoDialogView>();
				mapper.RegisterViewModelWithView<IDemoHandleWindowClosingViewModel, DemoHandleWindowClosingViewModel, DemoHandleWindowClosingView>();
				mapper.RegisterViewModelWithView<IDemoHandleWindowLoadingViewModel, DemoHandleWindowLoadingViewModel, DemoHandleWindowLoadingView>();
            } );

			IServiceProvider serviceProvider = services.BuildServiceProvider();

			IShellViewModel shellViewModel = serviceProvider.GetViewModel<IShellViewModel>();
			shellViewModel.Closed += (s, ea) => this.Shutdown();
			shellViewModel.Show();
		}
	}
}