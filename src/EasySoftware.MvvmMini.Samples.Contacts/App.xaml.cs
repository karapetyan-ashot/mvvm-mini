using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using EasySoftware.MvvmMini.Samples.Contacts.Factories;
using EasySoftware.MvvmMini.Samples.Contacts.Services;
using Unity;
using Unity.Injection;

namespace EasySoftware.MvvmMini.Samples.Contacts
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
      IUnityContainer _unityContainer;

      protected override void OnStartup(StartupEventArgs e)
      {
         this.ConfigureContainer();

         ILoginViewModel loginViewModel = this._unityContainer.Resolve<ILoginViewModel>(ViewModels.Login);
         loginViewModel.ShowDialog();
         if (loginViewModel.User != null)
         {

         }
         else
            this.Shutdown();
         
      }

      private void ConfigureContainer()
      {
         this._unityContainer = new UnityContainer().AddExtension(new Diagnostic());
         
         this._unityContainer.RegisterInstance(this._unityContainer);

         this._unityContainer.RegisterSingleton<IContactsService, ContactsMockService>();
         this._unityContainer.RegisterSingleton<IDialogFactory, DialogFactory>();
         
         this._unityContainer.RegisterType<IView, ViewWrapper<LoginView>>(ViewModels.Login);
         this._unityContainer.RegisterType<ILoginViewModel, LoginViewModel>(ViewModels.Login,
            new InjectionConstructor(
               new ResolvedParameter<IView>(ViewModels.Login), 
               new ResolvedParameter<IContactsService>()));

      }
   }
}
