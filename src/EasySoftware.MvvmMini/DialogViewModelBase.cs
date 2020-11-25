using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public abstract class DialogViewModelBase : WindowViewModelBase, IDialogViewModel
	{
		public DialogViewModelBase(IViewAdapter viewAdapter) : base(viewAdapter) { }

		public void ShowDialog()
		{
			this._viewAdapter.ShowDialog();
		}
	}
}
