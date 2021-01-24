using System.Windows;

using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;

namespace EasySoftware.MvvmMini.Samples.Notepad.Factories
{
	public class AppViewModelFactory : ViewModelFactory, IAppViewModelFactory
	{
		public IMessageBoxDialog CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons)
		{
			return this.ResolveViewModel<IMessageBoxDialog>(
				(nameof(message), message),
				(nameof(title), title),
				(nameof(buttons), buttons)
			);
		}
	}
}