namespace EasySoftware.MvvmMini.Core
{
	public interface IDialogWindowViewModel : IWindowViewModel
	{
		bool DialogResult { get; }
		bool ShowDialog();
	}
}
