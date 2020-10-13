using System.Windows;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox
{
	public interface IMessageBoxDialog : IDialogViewModel
	{
		MessageBoxResult DialogResult { get; }
	}
}
