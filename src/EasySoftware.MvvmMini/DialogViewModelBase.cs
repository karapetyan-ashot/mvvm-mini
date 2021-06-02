using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public abstract class DialogViewModelBase<T> : WindowViewModelBase, IDialogViewModel<T>
	{
		protected DialogViewModelBase(IViewAdapter viewAdapter) : base(viewAdapter) { }

		public abstract T DialogResult { get; protected set; }

		public void ShowDialog()
		{
			this._viewAdapter.ShowDialog();
		}
	}
}