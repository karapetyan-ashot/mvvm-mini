using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public abstract class ClosableViewModelBase : ViewModelBase, IClosableViewModel
	{
		public event EventHandler Closed;

      protected ClosableViewModelBase(IViewAdapter viewAdapter) : base(viewAdapter)
		{
			this.State = ViewModelState.Open;
			this.CloseCommand = new RelayCommand(this.Close, this.CanClose);
		}

		public ICommand CloseCommand { get; }

		private ViewModelState _state;
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

		private Task Close()
		{
			CancelEventArgs ea = new CancelEventArgs();
			this.OnClosing(ea);
			if (!ea.Cancel)
			{
				this.State = ViewModelState.Closing;
				this._viewAdapter.Close();
				this.State = ViewModelState.Closed;
			}

			return Task.CompletedTask;
		}

		protected virtual bool CanClose()
		{
			return true;
		}

		public virtual void OnClosing(CancelEventArgs e) { }
	}

}
