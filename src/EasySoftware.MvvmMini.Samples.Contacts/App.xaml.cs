using System;
using System.Windows;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using EasySoftware.MvvmMini.Samples.Contacts.Factories;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

using Unity;
using Unity.Injection;

namespace EasySoftware.MvvmMini.Samples.Contacts
{
	public partial class App : Application
	{
		IUnityContainer _unityContainer;

		protected override void OnStartup(StartupEventArgs e)
		{
			this.ConfigureContainer();

			IDialogFactory dialogFactory = this._unityContainer.Resolve<IDialogFactory>();
			ILoginViewModel loginViewModel = dialogFactory.CreateLoginDialog();
			loginViewModel.ShowDialog();
			if (loginViewModel.User != null)
			{
				IViewModelFactory factory = this._unityContainer.Resolve<IViewModelFactory>();
				IWindowViewModel mainViewModel = factory.CreateMainViewModel();

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
			this._unityContainer.RegisterSingleton<IDialogFactory, DialogFactory>();
			this._unityContainer.RegisterSingleton<IViewModelFactory, ViewModelFactory>();

			this._unityContainer.RegisterType<IView, ViewWrapper<LoginView>>(ViewModels.Login);
			this._unityContainer.RegisterType<ILoginViewModel, LoginViewModel>(ViewModels.Login,
			   new InjectionConstructor(
				  new ResolvedParameter<IView>(ViewModels.Login),
				  new ResolvedParameter<IContactsService>()));

			this._unityContainer.RegisterType<IView, ViewWrapper<MainView>>(ViewModels.Main);
			this._unityContainer.RegisterType<IWindowViewModel, MainViewModel>(ViewModels.Main,
				new InjectionConstructor(
					new ResolvedParameter<IView>(ViewModels.Main), 
					new ResolvedParameter<IContactsService>()));
		}
	}
}
