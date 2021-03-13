using System.Windows.Input;

namespace EasySoftware.MvvmMini.Core
{
	public interface IRelayCommand : ICommand
	{
		void RaiseCanExecuteChanged();
	}
}