using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public class ViewWrapper : IView  
	{
		public event EventHandler Loaded;

		private FrameworkElement _view { get; }

		public ViewWrapper(FrameworkElement view)
		{
			this._view = view ?? throw new ArgumentNullException(nameof(view));

			this._view.Loaded += (s, e) =>
				this.Loaded?.Invoke(this, EventArgs.Empty);

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
		}

		public void ShowDialog()
		{
			if (this._view is Window window)
			{
				window.Owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
				window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

				window.ShowDialog();
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
