
using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public class WindowViewModelBase : ClosableViewModelBase, IWindowViewModel
	{
		public WindowViewModelBase(IView view) : base(view)
		{
		}

		public void Show()
		{
			this._view.Show();
		}
	}
}
