using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Notepad.Factories;
using EasySoftware.MvvmMini.Samples.Notepad.Workplaces.Document;

using Unity;
using Unity.Injection;

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

         this._container.RegisterSingleton<IDialogFactory, DialogFactory>();
         this._container.RegisterSingleton<IViewModelFactory, ViewModelFactory>();

         this._container.RegisterType<IView, ViewWrapper<MainView>>(ViewModels.Main);
         this._container.RegisterType<IWindowViewModel, MainViewModel>(ViewModels.Main,
            new InjectionConstructor(
               new ResolvedParameter<IView>(ViewModels.Main),
               new ResolvedParameter<IViewModelFactory>()));

         this._container.RegisterType<IView, ViewWrapper<DocumentView>>(ViewModels.Document);
         this._container.RegisterType<IClosableViewModel, DocumentViewModel>(ViewModels.Document,
            new InjectionConstructor(
               new ResolvedParameter<IView>(ViewModels.Document),
               new ResolvedParameter<IDialogFactory>()));

         this._container.RegisterType<IView, ViewWrapper<MessageBoxView>>(ViewModels.MessageBox);
         this._container.RegisterType<IMessageBoxDialog, MessageBoxViewModel>(ViewModels.MessageBox,
            new InjectionConstructor(
               new ResolvedParameter<IView>(ViewModels.MessageBox),
               new OptionalParameter<string>(),
               new OptionalParameter<string>(),
               new OptionalParameter<MessageBoxButton>()));

      }

   }
}
