using System;
using System.Windows;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Notepad.Factories;

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

         IViewModelFactory viewModelFactory = this._container.Resolve<IViewModelFactory>();
         IWindowViewModel mainViewModel = viewModelFactory.CreateMainViewModel();

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

         this._container.RegisterInstance<IUnityContainer>(this._container);
         this._container.RegisterSingleton<IViewModelFactory, ViewModelFactory>();


      }

   }
}
