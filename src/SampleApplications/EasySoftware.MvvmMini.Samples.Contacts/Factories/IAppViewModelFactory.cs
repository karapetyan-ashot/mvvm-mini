using System.Windows;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Factories
{
	public interface IAppViewModelFactory : IViewModelFactory
	{
		IMessageBoxViewModel CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons);
	}
}
