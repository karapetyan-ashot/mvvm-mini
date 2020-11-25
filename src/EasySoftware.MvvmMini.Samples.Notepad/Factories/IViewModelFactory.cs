using System.Windows;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Notepad.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Notepad.Workplaces.Document;

namespace EasySoftware.MvvmMini.Samples.Notepad.Factories
{
	public interface IViewModelFactory
	{
		IMessageBoxDialog CreateMessageBoxDialog(string message, string title, MessageBoxButton buttons);
		IWindowViewModel CreateMainViewModel();
		IDocumentViewModel CreateDocumentViewModel();
	}
}
