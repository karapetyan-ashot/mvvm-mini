using System;
using System.Collections.Generic;
using System.Text;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using Unity;

namespace EasySoftware.MvvmMini.Samples.Contacts.Factories
{
   public class DialogFactory : IDialogFactory
   {
      private IUnityContainer _unityContainer;

      public DialogFactory(IUnityContainer unityContainer)
      {
         this._unityContainer = unityContainer ?? throw new ArgumentNullException(nameof(unityContainer));
      }

      public ILoginViewModel CreateLoginDialog()
      {
         return this._unityContainer.Resolve<ILoginViewModel>(ViewModels.Login);
      }
   }
}
