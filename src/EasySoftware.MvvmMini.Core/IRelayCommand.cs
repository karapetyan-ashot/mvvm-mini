using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySoftware.MvvmMini.Core
{
	public interface IRelayCommand : ICommand
	{
		bool IsRunning { get; }
		bool UseCommandManager { get; }
		void RaiseCanExecuteChanged();
		Task ExecuteAsync(object parameter);
	}
}