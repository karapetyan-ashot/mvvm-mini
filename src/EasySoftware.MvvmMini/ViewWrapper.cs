using System;
using System.ComponentModel;
using System.Windows;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public class ViewWrapper<T> : IView where T : FrameworkElement
	{
		public event EventHandler Loaded;

		public T View { get; }

		public ViewWrapper(T view)
		{
			this.View = view ?? throw new ArgumentNullException(nameof(view));

			this.View.Loaded += (s, e) =>
				this.Loaded?.Invoke(this, EventArgs.Empty);

			if (this.View is Window window)
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
			get => this.View.DataContext;
			set => this.View.DataContext = value;
		}

		public void Show()
		{
			if (this.View is Window window)
			{
				window.Show();
			}
		}

		public void ShowDialog()
		{
			if (this.View is Window window)
			{
				window.ShowDialog();
			}
		}

		public void Close()
		{
			if (this.View is Window window)
			{
				window.Close();
			}
		}
	}
}
