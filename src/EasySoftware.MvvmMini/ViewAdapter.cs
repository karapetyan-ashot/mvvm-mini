using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	/// <summary>
	/// Default <see cref="IViewAdapter"/> for WPF views. Works with any <see cref="FrameworkElement"/>;
	/// <see cref="Show"/>, <see cref="ShowDialog"/> and <see cref="Close"/> require the view to be a <see cref="Window"/>.
	/// </summary>
	/// <remarks>
	/// When the view is a window bound to a <see cref="ClosableViewModelBase"/>, closing the window calls
	/// <see cref="ClosableViewModelBase.OnClosing(CancelEventArgs)"/>, and a closed window moves the view model to <see cref="ViewModelState.Closed"/>.
	/// </remarks>
	public class ViewAdapter : IViewAdapter
	{
		private readonly FrameworkElement _view;

		private ViewAdapterSate _state;

		/// <inheritdoc/>
		public event EventHandler Loaded;
		/// <inheritdoc/>
		public event EventHandler Unloaded;

		/// <summary>
		/// Initializes a new adapter for the given view.
		/// </summary>
		/// <param name="view">The view to wrap.</param>
		/// <exception cref="ArgumentNullException"><paramref name="view"/> is <see langword="null"/>.</exception>
		public ViewAdapter(FrameworkElement view)
		{
			this._view = view ?? throw new ArgumentNullException(nameof(view));

			this._view.Loaded += (s, e) =>
			{
				if (this._state == ViewAdapterSate.NotLoaded)
				{
					this.Loaded?.Invoke(this, EventArgs.Empty);
					this._state = ViewAdapterSate.Loaded;
				}
			};

			this._view.Unloaded += (s, e) =>
			{
				if (this._state == ViewAdapterSate.Loaded)
				{
					this.Unloaded?.Invoke(this, EventArgs.Empty);
					this._state = ViewAdapterSate.Unloaded;
				}
			};

			if (this._view is Window window)
			{
				window.Closing += async (s, e) =>
				{
					if (window.DataContext is ClosableViewModelBase viewModel)
					{
						if (viewModel.State == ViewModelState.Open)
						{
							CancelEventArgs ea = new CancelEventArgs();
							await viewModel.OnClosing(ea);
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

		/// <inheritdoc/>
		public object DataContext
		{
			get => this._view.DataContext;
			set => this._view.DataContext = value;
		}

		/// <inheritdoc/>
		public object View { get => this._view; }

		/// <inheritdoc/>
		/// <exception cref="NotSupportedException">The view is not a <see cref="Window"/>.</exception>
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

		/// <summary>
		/// Shows the view as a modal dialog, owned by the currently active window of the application, and returns when it is closed.
		/// </summary>
		/// <exception cref="NotSupportedException">The view is not a <see cref="Window"/>.</exception>
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

		/// <summary>
		/// Closes the view if it is a <see cref="Window"/>; otherwise does nothing.
		/// </summary>
		public void Close()
		{
			if (this._view is Window window)
			{
				window.Close();
			}
		}

		private enum ViewAdapterSate
		{
			NotLoaded,
			Loaded,
			Unloaded
		}
	}
}