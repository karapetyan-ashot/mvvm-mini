using System;
using System.Windows;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
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

			IViewModelFactory viewModelFactory = this._unityContainer.Resolve<IViewModelFactory>();
			ILoginViewModel loginViewModel = viewModelFactory.CreateLoginDialog();
			loginViewModel.ShowDialog();
			if (loginViewModel.User != null)
			{
				IWindowViewModel mainViewModel = viewModelFactory.CreateMainViewModel();

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

			this._unityContainer.RegisterInstance(this._unityContainer);

			this._unityContainer.RegisterSingleton<IContactsService, ContactsMockService>();			
			this._unityContainer.RegisterSingleton<IViewModelFactory, ViewModelFactory>();
		}
	}
}
