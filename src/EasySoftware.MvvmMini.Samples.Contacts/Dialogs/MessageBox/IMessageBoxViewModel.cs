using System.Windows;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox
{
	public interface IMessageBoxViewModel : IDialogViewModel
	{
		MessageBoxResult DialogResult { get; }
	}
}
