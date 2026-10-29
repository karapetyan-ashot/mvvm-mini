namespace EasySoftware.MvvmMini.Core
{
    /// <summary>
    /// A view model whose view is shown as a modal dialog window.
    /// </summary>
    public interface IDialogViewModel : IWindowViewModel
    {
        /// <summary>
        /// Shows the view as a modal dialog and returns when the dialog is closed.
        /// </summary>
        void ShowDialog();
    }

    /// <summary>
    /// A modal dialog view model that produces a result.
    /// </summary>
    /// <typeparam name="T">The type of the dialog result.</typeparam>
    public interface IDialogViewModel<T> : IDialogViewModel
    {
        /// <summary>
        /// Gets the result of the dialog, available after it is closed.
        /// </summary>
        T DialogResult { get; }
    }
}