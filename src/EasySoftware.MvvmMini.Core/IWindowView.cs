namespace EasySoftware.MvvmMini.Core
{
	public interface IWindowView : IView
	{
		void Show();
		bool? ShowDialog();
		void Close();
	}
}
