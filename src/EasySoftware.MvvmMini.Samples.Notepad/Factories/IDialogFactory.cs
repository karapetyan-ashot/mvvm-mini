using System.Windows;

using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;

namespace EasySoftware.MvvmMini.Samples.Notepad.Factories
{
	public interface IDialogFactory
	{
		IMessageBoxDialog CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons);
	}

	
}
