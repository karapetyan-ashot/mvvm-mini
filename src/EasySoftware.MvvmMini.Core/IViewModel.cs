namespace EasySoftware.MvvmMini.Core
{
	/// <summary>
	/// A view model that owns its view.
	/// </summary>
	public interface IViewModel : IModel
	{
		/// <summary>
		/// Gets or sets the title of the view model, for example a window caption or a tab header.
		/// </summary>
		string Title { get; set; }

		/// <summary>
		/// Gets a value indicating whether the view model is busy with long-running work.
		/// </summary>
		bool IsBusy { get; }

		/// <summary>
		/// Gets the view of this view model, for use in data templates and content controls.
		/// </summary>
		object View { get; }

		/// <summary>
		/// Gets an identifier that distinguishes this view model instance from others.
		/// </summary>
		string UniqueId { get; }
	}
}