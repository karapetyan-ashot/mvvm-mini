using System;
using System.Windows;

using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Contacts.Factories;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

using Microsoft.Extensions.DependencyInjection;

namespace EasySoftware.MvvmMini.Samples.Contacts
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			IServiceProvider serviceProvider = ConfigureServices();

			IAppViewModelFactory viewModelFactory = serviceProvider.GetRequiredService<IAppViewModelFactory>();

			ILoginViewModel loginViewModel = viewModelFactory.CreateLoginDialog();
			loginViewModel.ShowDialog();
			if (loginViewModel.DialogResult != null)
			{
				IMainViewModel mainViewModel = viewModelFactory.CreateMainViewModel();

				mainViewModel.Closed += (s, ea) => this.Shutdown();
				mainViewModel.Show();
			}
			else
				this.Shutdown();
		}

		private IServiceProvider ConfigureServices()
		{
			IServiceCollection services = new ServiceCollection();

			services.AddSingleton<IAppViewModelFactory, AppViewModelFactory>();
			services.AddTransient<IContactsService, ContactsMockService>();

			services.AddMvvmMini(mapper => {
				mapper.RegisterViewModelWithView<ILoginViewModel, LoginViewModel, LoginView>();
                mapper.RegisterViewModelWithView<IMessageBoxViewModel, MessageBoxViewModel, MessageBoxView>();
                mapper.RegisterViewModelWithView<IContactEditorViewModel, ContactEditorViewModel, ContactEditorView>();
                mapper.RegisterViewModelWithView<IMainViewModel, MainViewModel, MainView>();
            });

			return services.BuildServiceProvider();
        }
	}
}
