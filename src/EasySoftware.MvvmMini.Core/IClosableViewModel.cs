using System;
using System.ComponentModel;
using System.Windows.Input;

namespace EasySoftware.MvvmMini.Core
{
	public interface IClosableViewModel : IViewModel
	{
		event EventHandler Closed;
		ICommand CloseCommand { get; }
		void OnClosing(CancelEventArgs e);
	}
}
