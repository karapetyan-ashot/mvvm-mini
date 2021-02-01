using System.Windows.Input;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.Samples.Notepad.Workplaces.Document
{
	public interface IDocumentViewModel: IClosableViewModel
	{
		ICommand SaveCommand { get; }
	}
}