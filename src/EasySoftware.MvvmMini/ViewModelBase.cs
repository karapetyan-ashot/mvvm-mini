using System.Threading.Tasks;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public abstract class ViewModelBase : ModelBase, IViewModel
	{
		protected IViewAdapter _viewAdapter;

		protected ViewModelBase(IViewAdapter viewAdapter)
		{
			this._viewAdapter = viewAdapter;
			this._viewAdapter.DataContext = this;
			this._viewAdapter.Loaded += async (s, e) => await this.Loaded();
			this._viewAdapter.Unloaded += async (s, e) => await this.Unloaded();
		}

		public string UniqueId { get; protected set; }

		private string _title;
		public string Title
		{
			get => this._title;
			set => SetProperty(ref this._title, value);
		}

		private bool _isBusy;
		public bool IsBusy
		{
			get => this._isBusy;
			set => SetProperty(ref this._isBusy, value);
		}

		public object View => this._viewAdapter.View;

		protected virtual Task Loaded() { return Task.CompletedTask; }
		protected virtual Task Unloaded() { return Task.CompletedTask; }
	}
}