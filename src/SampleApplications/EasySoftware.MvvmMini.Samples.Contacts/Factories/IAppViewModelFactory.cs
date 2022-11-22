using System.Windows;

using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Factories
{
    public interface IAppViewModelFactory
    {
        IMainViewModel CreateMainViewModel();
        ILoginViewModel CreateLoginDialog();
        IContactEditorViewModel CreateContactEditorDialog(ContactModel contact);
        IMessageBoxViewModel CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons);
    }
}
