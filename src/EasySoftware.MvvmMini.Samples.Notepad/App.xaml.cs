using System;
using System.Windows;

using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Notepad.Factories;
using EasySoftware.MvvmMini.Samples.Notepad.Workplaces.Document;

using Unity;

namespace EasySoftware.MvvmMini.Samples.Notepad
{
	public partial class App : Application
	{
		IUnityContainer _container;
		public App()
		{
			this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			ConfigureContainer();

			IAppViewModelFactory viewModelFactory = this._container.Resolve<IAppViewModelFactory>();
			IMainViewModel mainViewModel = viewModelFactory.ResolveViewModel<IMainViewModel>();

			mainViewModel.Closed += MainViewModel_Closed;
			mainViewModel.Show();
		}

		private void MainViewModel_Closed(object sender, EventArgs e)
		{
			this.Shutdown();
		}

		private void ConfigureContainer()
		{
			this._container = new UnityContainer().AddExtension(new Diagnostic());

			IAppViewModelFactory viewModelFactory = new AppViewModelFactory(this._container);

			this._container.RegisterInstance<IAppViewModelFactory>(viewModelFactory);

			viewModelFactory.RegisterViewModelWithView<IMainViewModel, MainViewModel, MainView>();
			viewModelFactory.RegisterViewModelWithView<IDocumentViewModel, DocumentViewModel, DocumentView>();
			viewModelFactory.RegisterViewModelWithView<IMessageBoxDialog, MessageBoxViewModel, MessageBoxView>();
		}
	}
}