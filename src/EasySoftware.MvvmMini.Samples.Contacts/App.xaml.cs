using System;
using System.Windows;

using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Contacts.Factories;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

using Unity;

namespace EasySoftware.MvvmMini.Samples.Contacts
{
	public partial class App : Application
	{
		IUnityContainer _unityContainer;

		protected override void OnStartup(StartupEventArgs e)
		{
			this.ConfigureContainer();

			IAppViewModelFactory viewModelFactory = this._unityContainer.Resolve<IAppViewModelFactory>();
			ILoginViewModel loginViewModel = viewModelFactory.ResolveViewModel<ILoginViewModel>();
			loginViewModel.ShowDialog();
			if (loginViewModel.User != null)
			{
				IMainViewModel mainViewModel = viewModelFactory.ResolveViewModel<IMainViewModel>();

				mainViewModel.Closed += MainViewModel_Closed;
				mainViewModel.Show();
			}
			else
				this.Shutdown();
		}

		private void MainViewModel_Closed(object sender, EventArgs e)
		{
			this.Shutdown();
		}

		private void ConfigureContainer()
		{
			this._unityContainer = new UnityContainer().AddExtension(new Diagnostic());

			IAppViewModelFactory viewModelFactory = new AppViewModelFactory(this._unityContainer);
			this._unityContainer.RegisterInstance<IAppViewModelFactory>(viewModelFactory);

			this._unityContainer.RegisterSingleton<IContactsService, ContactsMockService>();

			viewModelFactory.RegisterViewModelWithView<ILoginViewModel, LoginViewModel, LoginView>();
			viewModelFactory.RegisterViewModelWithView<IMessageBoxViewModel, MessageBoxViewModel, MessageBoxView>();
			viewModelFactory.RegisterViewModelWithView<IContactEditorViewModel, ContactEditorViewModel, ContactEditorView>();
			viewModelFactory.RegisterViewModelWithView<IMainViewModel, MainViewModel, MainView>();

		}
	}
}
