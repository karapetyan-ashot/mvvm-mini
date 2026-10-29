using System.Threading.Tasks;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	/// <summary>
	/// Base class for view models. Binds itself to its view and exposes the view's load and unload events
	/// as the overridable <see cref="Loaded"/> and <see cref="Unloaded"/> methods.
	/// </summary>
	public abstract class ViewModelBase : ModelBase, IViewModel
	{
		/// <summary>
		/// The adapter of the view that belongs to this view model.
		/// </summary>
		protected IViewAdapter _viewAdapter;

		/// <summary>
		/// Initializes a new instance and sets it as the data context of the view.
		/// </summary>
		/// <param name="viewAdapter">The adapter of the view that belongs to this view model.</param>
		protected ViewModelBase(IViewAdapter viewAdapter)
		{
			this._viewAdapter = viewAdapter;
			this._viewAdapter.DataContext = this;
			this._viewAdapter.Loaded += async (s, e) => await this.Loaded();
			this._viewAdapter.Unloaded += async (s, e) => await this.Unloaded();
		}

		/// <inheritdoc/>
		public string UniqueId { get; protected set; }

		private string _title;

		/// <inheritdoc/>
		public string Title
		{
			get => this._title;
			set => SetProperty(ref this._title, value);
		}

		private bool _isBusy;

		/// <summary>
		/// Gets or sets a value indicating whether the view model is busy with long-running work.
		/// </summary>
		public bool IsBusy
		{
			get => this._isBusy;
			set => SetProperty(ref this._isBusy, value);
		}

		/// <inheritdoc/>
		public object View => this._viewAdapter.View;

		/// <summary>
		/// Called the first time the view is loaded. Override it to load data. Does nothing by default.
		/// </summary>
		/// <returns>A task that completes when loading is done.</returns>
		protected virtual Task Loaded() { return Task.CompletedTask; }

		/// <summary>
		/// Called when the view is unloaded. Does nothing by default.
		/// </summary>
		/// <returns>A task that completes when the work is done.</returns>
		protected virtual Task Unloaded() { return Task.CompletedTask; }
	}
}