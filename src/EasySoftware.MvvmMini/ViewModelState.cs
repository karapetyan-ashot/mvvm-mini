namespace EasySoftware.MvvmMini
{
	/// <summary>
	/// The lifecycle state of a closable view model.
	/// </summary>
	public enum ViewModelState
	{
		/// <summary>
		/// The view model is open and in use.
		/// </summary>
		Open,

		/// <summary>
		/// Closing was confirmed in <see cref="ClosableViewModelBase.OnClosing(System.ComponentModel.CancelEventArgs)"/> and the view is being closed.
		/// </summary>
		Closing,

		/// <summary>
		/// The view model is closed and its lifecycle has ended.
		/// </summary>
		Closed,
	}
}
