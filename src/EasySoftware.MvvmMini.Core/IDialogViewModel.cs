namespace EasySoftware.MvvmMini.Core
{
	public interface IDialogViewModel<T> : IWindowViewModel
	{
		T DialogResult { get; }
		
		void ShowDialog();		
	}
}