using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public class DialogViewModelBase : WindowViewModelBase, IDialogViewModel
	{
		public DialogViewModelBase(IView view) : base(view)
		{
		}

		public void ShowDialog()
		{
			this._view.ShowDialog();
		}
	}
}
