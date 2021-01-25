using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public class ViewAdapter : IViewAdapter
	{
		private bool _isLoaded;

		public event EventHandler Loaded;

		private readonly FrameworkElement _view;

		public ViewAdapter(FrameworkElement view)
		{
			this._view = view ?? throw new ArgumentNullException(nameof(view));

			this._view.Loaded += (s, e) =>
			{
				if (!this._isLoaded)
				{
					this.Loaded?.Invoke(this, EventArgs.Empty);
					this._isLoaded = true;
				}
			};

			if (this._view is Window window)
			{
				window.Closing += (s, e) =>
				{
					if (window.DataContext is ClosableViewModelBase viewModel)
					{
						if (viewModel.State == ViewModelState.Open)
						{
							CancelEventArgs ea = new CancelEventArgs();
							viewModel.OnClosing(ea);
							e.Cancel = ea.Cancel;
						}
					}
				};

				window.Closed += (s, e) =>
				{
					if (window.DataContext is ClosableViewModelBase viewModel)
					{
						viewModel.State = ViewModelState.Closed;
					}
				};
			}
		}

		public object DataContext
		{
			get => this._view.DataContext;
			set => this._view.DataContext = value;
		}

		public object View { get => this._view; }

		public void Show()
		{
			if (this._view is Window window)
			{
				window.Show();
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public void ShowDialog()
		{
			if (this._view is Window window)
			{
				window.Owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
				window.WindowStartupLocation = window.Owner == null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;

				window.ShowDialog();
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public void Close()
		{
			if (this._view is Window window)
			{
				window.Close();
			}
		}
	}
}