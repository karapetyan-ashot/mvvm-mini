using System.Windows;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Factories
{
	public interface IViewModelFactory
	{
		IWindowViewModel CreateMainViewModel();
		ILoginViewModel CreateLoginDialog();
		IMessageBoxDialog CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons);
		IContactEditor CreateContactEditorDialog(Contact contact);
	}
}
