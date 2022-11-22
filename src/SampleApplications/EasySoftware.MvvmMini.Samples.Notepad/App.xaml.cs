using System;
using System.Windows;

using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Notepad.Workplaces.Document;

using Microsoft.Extensions.DependencyInjection;

namespace EasySoftware.MvvmMini.Samples.Notepad
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            IServiceCollection services = new ServiceCollection();
            
            services.AddMvvmMini(mapper => {
                mapper.RegisterViewModelWithView<IMainViewModel, MainViewModel, MainView>();
                mapper.RegisterViewModelWithView<IDocumentViewModel, DocumentViewModel, DocumentView>();
                mapper.RegisterViewModelWithView<IMessageBoxViewModel, MessageBoxViewModel, MessageBoxView>();
            });
            var sp = services.BuildServiceProvider();

            var mainVM = sp.GetViewModel<IMainViewModel>();
            mainVM.Closed += (s, ea) => this.Shutdown();
            mainVM.Show();
        }
    }
}