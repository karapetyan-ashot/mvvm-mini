
using System.Threading.Tasks;
using System.Windows;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public class ViewModelBase : BindableBase, IViewModel
	{
		protected IView _view;

		public ViewModelBase(IView view)
		{
			this._view = view;
			this._view.DataContext = this;
			this._view.Loaded += async (s, e) => await this.Loaded();
		}

		private string _title;
		public string Title
		{
			get { return this._title; }
			set
			{
				if (this._title != value)
				{
					this._title = value;
					this.RaisePropertyChanged();
				}
			}
		}

		private bool _isBusy;
		public bool IsBusy
		{
			get => this._isBusy;
			set
			{
				if (this._isBusy != value)
				{
					this._isBusy = value;
					RaisePropertyChanged();
				}
			}
		}

		public object View => this._view.View;

		protected virtual Task Loaded() { return Task.CompletedTask; }
	}
}
