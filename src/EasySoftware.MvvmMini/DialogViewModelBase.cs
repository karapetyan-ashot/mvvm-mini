using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public abstract class DialogViewModelBase : WindowViewModelBase, IDialogViewModel
	{
      protected DialogViewModelBase(IViewAdapter viewAdapter) : base(viewAdapter) { }

		public void ShowDialog()
		{
			this._viewAdapter.ShowDialog();
		}
	}
}
