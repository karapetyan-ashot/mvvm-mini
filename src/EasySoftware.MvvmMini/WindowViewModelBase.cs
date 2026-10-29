using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	/// <summary>
	/// Base class for view models whose view is a window.
	/// </summary>
	public abstract class WindowViewModelBase : ClosableViewModelBase, IWindowViewModel
	{
      /// <summary>
      /// Initializes a new instance of the <see cref="WindowViewModelBase"/> class.
      /// </summary>
      /// <param name="viewAdapter">The adapter of the window that belongs to this view model.</param>
      protected WindowViewModelBase(IViewAdapter viewAdapter) : base(viewAdapter) { }

		/// <inheritdoc/>
		public void Show()
		{
			this._viewAdapter.Show();
		}
	}
}