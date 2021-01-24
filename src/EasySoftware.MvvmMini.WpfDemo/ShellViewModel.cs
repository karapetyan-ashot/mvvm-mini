using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.WpfDemo
{
	public interface IShellViewModel : IWindowViewModel { }

	public class ShellViewModel : WindowViewModelBase, IShellViewModel
	{
		private IViewModelFactory _viewModelFactory;

		public ShellViewModel(IViewAdapter viewAdapter, IViewModelFactory viewModelFactory) : base(viewAdapter)
		{
			this._viewModelFactory = viewModelFactory;

			this.Title = "MvvmMini.Demo";
		}
	}
}