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
        private IServiceProvider _serviceProvider;
        private bool _isRestarting = false;
        private IMainViewModel _mainViewModel;
        protected override void OnStartup(StartupEventArgs e)
        {
            _serviceProvider = ConfigureServices();

            StartApp();

        }

        private void StartApp()
        {
            IAppViewModelFactory viewModelFactory = _serviceProvider.GetRequiredService<IAppViewModelFactory>();

            ILoginViewModel loginViewModel = viewModelFactory.CreateLoginDialog();
            loginViewModel.ShowDialog();
            if (loginViewModel.DialogResult != null)
            {
                _mainViewModel = viewModelFactory.CreateMainViewModel();
                
                _mainViewModel.Closed += (s, ea) =>
                {
                    if (!_isRestarting)
                        this.Shutdown();
                };

                _mainViewModel.Show();
            }
            else
                this.Shutdown();
        }

        private IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IAppViewModelFactory, AppViewModelFactory>();
            services.AddTransient<IContactsService, ContactsMockService>();

            services.AddMvvmMini(mapper =>
            {
                mapper.RegisterViewModelWithView<ILoginViewModel, LoginViewModel, LoginView>();
                mapper.RegisterViewModelWithView<IMessageBoxViewModel, MessageBoxViewModel, MessageBoxView>();
                mapper.RegisterViewModelWithView<IContactEditorViewModel, ContactEditorViewModel, ContactEditorView>();
                mapper.RegisterViewModelWithView<IMainViewModel, MainViewModel, MainView>();
            });

            return services.BuildServiceProvider();
        }

        public void Restart()
        {
            _isRestarting = true;
            _mainViewModel.CloseCommand.Execute(null);            
            this.StartApp();

            _isRestarting = false;
        }
    }
}
