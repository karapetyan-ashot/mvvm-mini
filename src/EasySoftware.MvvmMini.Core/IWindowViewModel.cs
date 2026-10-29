namespace EasySoftware.MvvmMini.Core
{
	/// <summary>
	/// A view model whose view is a window.
	/// </summary>
	public interface IWindowViewModel : IClosableViewModel
	{
		/// <summary>
		/// Shows the view as a non-modal window.
		/// </summary>
		void Show();
	}
}
