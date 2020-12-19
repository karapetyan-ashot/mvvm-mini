using System.ComponentModel;

namespace EasySoftware.MvvmMini.Core
{
	public interface IViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Unique ID to separate items in collections
		/// </summary>
		string UniqueId { get; }

		/// <summary>
		/// ViewModel's Title
		/// </summary>
		string Title { get; set; }

		/// <summary>
		/// To indicate that model is busy
		/// </summary>
		bool IsBusy { get; }

		/// <summary>
		/// View to bind in DataTemplates
		/// </summary>
		object View { get; }
	}
}
