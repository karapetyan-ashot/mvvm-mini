using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public class DialogViewModelBase : WindowViewModelBase, IDialogViewModel
	{
		public DialogViewModelBase(IView view) : base(view)
		{
		}

		public bool ShowDialog()
		{
			var result = this._view.ShowDialog();
			return result ?? false;
		}
	}
}
