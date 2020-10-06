using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySoftware.MvvmMini
{
	public class RelayCommand<T> : ICommand where T : class
	{

		private readonly Func<T, Task> _execute;
		private readonly Func<bool> _canExecute;
		private bool _isLoad = false;

		public RelayCommand(Func<T, Task> execute, Func<bool> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			if (this._isLoad)
				return false;

			if (_canExecute != null)
				return _canExecute();

			return true;
		}

		public async void Execute(object parameter)
		{
			this._isLoad = true;
			await _execute(parameter as T);
			this._isLoad = false;
			this.RaiseCanExecuteChanged();
		}

		public Task ExecuteAsync(object parameter)
		{
			return Task.FromResult(this._execute);
		}

		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
				LocalCanExecuteChanged += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
				LocalCanExecuteChanged -= value;
			}
		}

		public event EventHandler LocalCanExecuteChanged;

		public void RaiseCanExecuteChanged()
		{
			LocalCanExecuteChanged?.Invoke(this, new EventArgs());
		}

	}

	public class RelayCommand : RelayCommand<object>
	{
		private string login;
		private Func<bool> canLogin;
		private ICommand save;
		private Func<bool> canSave;

		public RelayCommand(Func<Task> execute, Func<bool> canExecute = null)
			: base(o => execute(), canExecute)
		{

		}

	}

}
