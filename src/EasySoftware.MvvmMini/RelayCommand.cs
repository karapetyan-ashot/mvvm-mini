using System;
using System.Threading.Tasks;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	/// <summary>
	/// Base class for asynchronous relay commands. While a command is running, <see cref="CanExecute(object)"/> returns <see langword="false"/>.
	/// </summary>
	public abstract class RelayCommandBase : IRelayCommand
	{
		/// <inheritdoc/>
		public bool UseCommandManager { get; protected set; }
		/// <inheritdoc/>
		public bool IsRunning { get; protected set; }

		private event EventHandler _canExecuteChanged;
		/// <summary>
		/// Occurs when the result of <see cref="CanExecute(object)"/> may have changed. When <see cref="UseCommandManager"/>
		/// is <see langword="true"/>, handlers are attached to <see cref="CommandManager.RequerySuggested"/>.
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (this.UseCommandManager)
					CommandManager.RequerySuggested += value;
				else
					this._canExecuteChanged += value;
			}
			remove
			{
				if (this.UseCommandManager)
					CommandManager.RequerySuggested -= value;
				else
					this._canExecuteChanged -= value;
			}
		}

		/// <summary>
		/// Executes the command without waiting for it to finish. Use <see cref="ExecuteAsync(object)"/> to await it.
		/// </summary>
		/// <param name="parameter">The command parameter.</param>
		public abstract void Execute(object parameter);
		/// <inheritdoc/>
		public abstract Task ExecuteAsync(object parameter);

		/// <summary>
		/// Determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">The command parameter.</param>
		/// <returns><see langword="true"/> if the command can execute; otherwise <see langword="false"/>.</returns>
		public abstract bool CanExecute(object parameter);

		/// <inheritdoc/>
		public void RaiseCanExecuteChanged()
		{
			if (this.UseCommandManager)
				CommandManager.InvalidateRequerySuggested();
			else
				this._canExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	/// <summary>
	/// An asynchronous command that takes a parameter of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the command parameter.</typeparam>
	public class RelayCommand<T> : RelayCommandBase
	{
		private readonly Func<T, Task> _execute;
		private readonly Func<T, bool> _canExecute;

		/// <summary>
		/// Initializes a command that can always execute.
		/// </summary>
		/// <param name="execute">The work to run when the command executes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="execute"/> is <see langword="null"/>.</exception>
		public RelayCommand(Func<T, Task> execute)
		   : this(execute, null, false) { }

		/// <summary>
		/// Initializes a command whose availability is re-evaluated through <see cref="CommandManager"/>.
		/// </summary>
		/// <param name="execute">The work to run when the command executes.</param>
		/// <param name="canExecute">Returns whether the command can execute for a given parameter.</param>
		/// <exception cref="ArgumentNullException"><paramref name="execute"/> is <see langword="null"/>.</exception>
		public RelayCommand(Func<T, Task> execute, Func<T, bool> canExecute)
		   : this(execute, canExecute, true) { }


		/// <summary>
		/// Initializes a command.
		/// </summary>
		/// <param name="execute">The work to run when the command executes.</param>
		/// <param name="canExecute">Returns whether the command can execute for a given parameter, or <see langword="null"/> if it always can.</param>
		/// <param name="useCommandManager"><see langword="true"/> to re-evaluate <see cref="RelayCommandBase.CanExecute(object)"/> through <see cref="CommandManager"/>;
		/// <see langword="false"/> to re-evaluate it only when <see cref="RelayCommandBase.RaiseCanExecuteChanged"/> is called.</param>
		/// <exception cref="ArgumentNullException"><paramref name="execute"/> is <see langword="null"/>.</exception>
		public RelayCommand(Func<T, Task> execute, Func<T, bool> canExecute, bool useCommandManager)
		{
			this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
			this._canExecute = canExecute;
			this.UseCommandManager = useCommandManager;
		}

		public override async void Execute(object parameter)
		{
			await ExecuteAsync(parameter);
		}

		public override async Task ExecuteAsync(object parameter)
		{
			this.IsRunning = true;
			await _execute((T)parameter);
			this.IsRunning = false;
			this.RaiseCanExecuteChanged();
		}

		public override bool CanExecute(object parameter)
		{
			if (this.IsRunning)
				return false;

			if (_canExecute == null)
				return true;

			return _canExecute((T)parameter);
		}
	}

	/// <summary>
	/// An asynchronous command without a parameter.
	/// </summary>
	public class RelayCommand : RelayCommandBase
	{
		private readonly Func<Task> _execute;
		private readonly Func<bool> _canExecute;

		/// <summary>
		/// Initializes a command that can always execute.
		/// </summary>
		/// <param name="execute">The work to run when the command executes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="execute"/> is <see langword="null"/>.</exception>
		public RelayCommand(Func<Task> execute)
		   : this(execute, null, false) { }

		/// <summary>
		/// Initializes a command whose availability is re-evaluated through <see cref="CommandManager"/>.
		/// </summary>
		/// <param name="execute">The work to run when the command executes.</param>
		/// <param name="canExecute">Returns whether the command can execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="execute"/> is <see langword="null"/>.</exception>
		public RelayCommand(Func<Task> execute, Func<bool> canExecute)
		   : this(execute, canExecute, true) { }

		/// <summary>
		/// Initializes a command.
		/// </summary>
		/// <param name="execute">The work to run when the command executes.</param>
		/// <param name="canExecute">Returns whether the command can execute, or <see langword="null"/> if it always can.</param>
		/// <param name="useCommandManager"><see langword="true"/> to re-evaluate <see cref="RelayCommandBase.CanExecute(object)"/> through <see cref="CommandManager"/>;
		/// <see langword="false"/> to re-evaluate it only when <see cref="RelayCommandBase.RaiseCanExecuteChanged"/> is called.</param>
		/// <exception cref="ArgumentNullException"><paramref name="execute"/> is <see langword="null"/>.</exception>
		public RelayCommand(Func<Task> execute, Func<bool> canExecute, bool useCommandManager)
		{
			this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
			this._canExecute = canExecute;
			this.UseCommandManager = useCommandManager;
		}

		public override async void Execute(object parameter)
		{
			await ExecuteAsync(parameter);
		}

		public override async Task ExecuteAsync(object parameter)
		{
			this.IsRunning = true;
			await _execute();
			this.IsRunning = false;
			this.RaiseCanExecuteChanged();
		}

		public override bool CanExecute(object parameter)
		{
			if (this.IsRunning)
				return false;

			if (_canExecute == null)
				return true;

			return _canExecute();
		}
	}
}