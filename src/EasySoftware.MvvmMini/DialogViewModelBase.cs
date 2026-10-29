using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{

    /// <summary>
    /// Base class for view models whose view is shown as a modal dialog window.
    /// </summary>
    public abstract class DialogViewModelBase : WindowViewModelBase, IDialogViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogViewModelBase"/> class.
        /// </summary>
        /// <param name="viewAdapter">The adapter of the dialog window that belongs to this view model.</param>
        protected DialogViewModelBase(IViewAdapter viewAdapter) : base(viewAdapter) { }

        /// <inheritdoc/>
        public void ShowDialog()
        {
            this._viewAdapter.ShowDialog();
        }
    }

    /// <summary>
    /// Base class for modal dialog view models that produce a result.
    /// </summary>
    /// <typeparam name="T">The type of the dialog result.</typeparam>
    public abstract class DialogViewModelBase<T> : DialogViewModelBase, IDialogViewModel<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DialogViewModelBase{T}"/> class.
		/// </summary>
		/// <param name="viewAdapter">The adapter of the dialog window that belongs to this view model.</param>
		protected DialogViewModelBase(IViewAdapter viewAdapter) : base(viewAdapter) { }

		/// <summary>
		/// Gets or sets the result of the dialog, available after it is closed.
		/// </summary>
		public abstract T DialogResult { get; protected set; }
	}
}