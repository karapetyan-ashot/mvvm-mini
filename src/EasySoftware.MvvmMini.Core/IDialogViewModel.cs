namespace EasySoftware.MvvmMini.Core
{
    public interface IDialogViewModel : IWindowViewModel
    {
        void ShowDialog();
    }

    public interface IDialogViewModel<T> : IDialogViewModel
    {
        T DialogResult { get; }
    }
}