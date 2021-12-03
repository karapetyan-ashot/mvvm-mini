using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySoftware.MvvmMini.Core
{
	public interface IClosableViewModel : IViewModel
	{
		event EventHandler Closed;
		ICommand CloseCommand { get; }
		Task OnClosing(CancelEventArgs e);
	}
}
