using System.Windows;
using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.WpfDemo
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

			IViewModelFactory viewModelFactory = new ViewModelFactory();
			
			viewModelFactory.RegisterViewModelWithView<IShellViewModel, ShellViewModel, ShellView>();

			IShellViewModel shellViewModel = viewModelFactory.ResolveViewModel<IShellViewModel>();
			shellViewModel.Closed += (s, ea) => this.Shutdown();
			shellViewModel.Show();
		}
	}
}
