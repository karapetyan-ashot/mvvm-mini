namespace EasySoftware.MvvmMini.Core
{
	public interface IViewModel : IModel
	{
		/// <summary>
		/// ViewModel's Title.
		/// </summary>
		string Title { get; set; }

		/// <summary>
		/// IsBusy indicator.
		/// </summary>
		bool IsBusy { get; }

		/// <summary>
		/// View to bind in DataTemplates.
		/// </summary>
		object View { get; }
	}
}