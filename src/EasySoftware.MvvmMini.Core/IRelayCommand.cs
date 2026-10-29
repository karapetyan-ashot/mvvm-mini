using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySoftware.MvvmMini.Core
{
	/// <summary>
	/// An <see cref="ICommand"/> that runs asynchronous work and cannot be executed again while it is running.
	/// </summary>
	public interface IRelayCommand : ICommand
	{
		/// <summary>
		/// Gets a value indicating whether the command is currently executing.
		/// </summary>
		bool IsRunning { get; }

		/// <summary>
		/// Gets a value indicating whether <see cref="ICommand.CanExecuteChanged"/> is driven by
		/// <see cref="CommandManager.RequerySuggested"/> instead of explicit calls to <see cref="RaiseCanExecuteChanged"/>.
		/// </summary>
		bool UseCommandManager { get; }

		/// <summary>
		/// Notifies bound controls that the result of <see cref="ICommand.CanExecute(object)"/> may have changed.
		/// </summary>
		void RaiseCanExecuteChanged();

		/// <summary>
		/// Executes the command and returns a task that completes when the work is done.
		/// </summary>
		/// <param name="parameter">The command parameter.</param>
		/// <returns>A task that represents the execution.</returns>
		Task ExecuteAsync(object parameter);
	}
}