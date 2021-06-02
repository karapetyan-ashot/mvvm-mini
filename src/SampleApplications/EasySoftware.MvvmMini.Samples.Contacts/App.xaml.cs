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
		protected override void OnStartup(StartupEventArgs e)
		{
			IAppViewModelFactory viewModelFactory = this.ConfigureContainer();
			ILoginViewModel loginViewModel = viewModelFactory.ResolveViewModel<ILoginViewModel>();
			loginViewModel.ShowDialog();
			if (loginViewModel.DialogResult != null)
			{
				IMainViewModel mainViewModel = viewModelFactory.ResolveViewModel<IMainViewModel>();

				mainViewModel.Closed += (s, ea) => this.Shutdown();
				mainViewModel.Show();
			}
			else
				this.Shutdown();
		}

		private IAppViewModelFactory ConfigureContainer()
		{
			IAppViewModelFactory viewModelFactory = new AppViewModelFactory();

			viewModelFactory.Container.RegisterInstance<IAppViewModelFactory>(viewModelFactory);
			viewModelFactory.Container.RegisterSingleton<IContactsService, ContactsMockService>();

			viewModelFactory.RegisterViewModelWithView<ILoginViewModel, LoginViewModel, LoginView>();
			viewModelFactory.RegisterViewModelWithView<IMessageBoxViewModel, MessageBoxViewModel, MessageBoxView>();
			viewModelFactory.RegisterViewModelWithView<IContactEditorViewModel, ContactEditorViewModel, ContactEditorView>();
			viewModelFactory.RegisterViewModelWithView<IMainViewModel, MainViewModel, MainView>();

			return viewModelFactory;
		}
	}
}
