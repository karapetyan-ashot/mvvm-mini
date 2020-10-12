using System;
using System.Collections.Generic;
using System.Text;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using Unity;

namespace EasySoftware.MvvmMini.Samples.Contacts.Factories
{
   public interface IDialogFactory
   {
      ILoginViewModel CreateLoginDialog();
   }
}
