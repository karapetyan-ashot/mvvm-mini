using System;
using System.Windows.Input;

namespace EasySoftware.MvvmMini.Core
{
	public interface IClosableViewModel : IViewModel
	{
		string Title { get; set; }
		ICommand CloseCommand { get; }
		event EventHandler Closed;
		void OnClosed();
	}
}
