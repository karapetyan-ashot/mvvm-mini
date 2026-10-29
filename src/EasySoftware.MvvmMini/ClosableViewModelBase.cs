using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	/// <summary>
	/// Base class for view models with a lifecycle that ends when they are closed, such as a window,
	/// a dialog or a document tab that the user opens, works with and then closes.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Closing starts from <see cref="CloseCommand"/> or, when the view is a window, from the user closing the window.
	/// In both cases <see cref="OnClosing(CancelEventArgs)"/> is called first. Override it to ask, for example,
	/// "Do you want to save changes?" and cancel closing when the user chooses Cancel.
	/// </para>
	/// <para>
	/// When closing goes ahead, the view is closed (if it is a window), <see cref="State"/> becomes
	/// <see cref="ViewModelState.Closed"/> and <see cref="Closed"/> is raised.
	/// </para>
	/// </remarks>
	public abstract class ClosableViewModelBase : ViewModelBase, IClosableViewModel
	{
		/// <inheritdoc/>
		public event EventHandler Closed;

      /// <summary>
      /// Initializes a new instance in the <see cref="ViewModelState.Open"/> state.
      /// </summary>
      /// <param name="viewAdapter">The adapter of the view that belongs to this view model.</param>
      protected ClosableViewModelBase(IViewAdapter viewAdapter) : base(viewAdapter)
		{
			this.State = ViewModelState.Open;
			this.CloseCommand = new RelayCommand(this.Close, this.CanClose);
		}

		/// <summary>
		/// Gets the command that asks the view model to close. Bind it to a Close button, a tab's close icon
		/// or a menu item. Closing can be cancelled in <see cref="OnClosing(CancelEventArgs)"/>, and the command
		/// is available only while <see cref="CanClose"/> returns <see langword="true"/>.
		/// </summary>
		public ICommand CloseCommand { get; }

		private ViewModelState _state;

		/// <summary>
		/// Gets or sets the lifecycle state of the view model. Setting it to <see cref="ViewModelState.Closed"/> raises <see cref="Closed"/>.
		/// </summary>
		public ViewModelState State
		{
			get => this._state;
			set
			{
				if (this._state != value)
				{
					this._state = value;
					if (this._state == ViewModelState.Closed)
						this.Closed?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		private async Task Close()
		{
			CancelEventArgs ea = new CancelEventArgs();
			await this.OnClosing(ea);
			if (!ea.Cancel)
			{
				this.State = ViewModelState.Closing;
				this._viewAdapter.Close();
				this.State = ViewModelState.Closed;
			}
		}

		/// <summary>
		/// Determines whether <see cref="CloseCommand"/> can execute, for example to disable closing while a save is in progress.
		/// Returns <see langword="true"/> by default. Unlike <see cref="OnClosing(CancelEventArgs)"/>, it does not ask the user anything.
		/// </summary>
		/// <returns><see langword="true"/> if the view model can be closed; otherwise <see langword="false"/>.</returns>
		protected virtual bool CanClose()
		{
			return true;
		}

		/// <summary>
		/// Called before the view model is closed, either by <see cref="CloseCommand"/> or by the user closing the window.
		/// Override it to decide whether closing may go ahead, for example to ask the user whether to save
		/// unsaved changes, and set <see cref="CancelEventArgs.Cancel"/> to <see langword="true"/> to stay open.
		/// Does nothing by default.
		/// </summary>
		/// <param name="e">The event data. Set <see cref="CancelEventArgs.Cancel"/> to <see langword="true"/> to cancel closing.</param>
		/// <returns>A task that completes when the view model has decided whether to close.</returns>
		public virtual Task OnClosing(CancelEventArgs e) { return Task.CompletedTask; }
	}

}
