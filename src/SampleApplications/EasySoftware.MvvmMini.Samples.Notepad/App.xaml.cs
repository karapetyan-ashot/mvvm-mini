using System.Windows;

using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Notepad.Factories;
using EasySoftware.MvvmMini.Samples.Notepad.Workplaces.Document;
using Unity;

namespace EasySoftware.MvvmMini.Samples.Notepad
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

			IAppViewModelFactory viewModelFactory = CreateAndConfigureFactory();
						
			IMainViewModel mainViewModel = viewModelFactory.ResolveViewModel<IMainViewModel>();
			mainViewModel.Closed += (s, ea) => this.Shutdown();
			mainViewModel.Show();
		}

		private IAppViewModelFactory CreateAndConfigureFactory()
		{			
			IAppViewModelFactory viewModelFactory = new AppViewModelFactory();
			viewModelFactory.Container.RegisterInstance<IAppViewModelFactory>(viewModelFactory);

			viewModelFactory.RegisterViewModelWithView<IMainViewModel, MainViewModel, MainView>();
			viewModelFactory.RegisterViewModelWithView<IDocumentViewModel, DocumentViewModel, DocumentView>();
			viewModelFactory.RegisterViewModelWithView<IMessageBoxDialog, MessageBoxViewModel, MessageBoxView>();
			
			return viewModelFactory;
		}
	}
}