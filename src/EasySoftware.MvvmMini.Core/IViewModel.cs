using System.ComponentModel;

namespace EasySoftware.MvvmMini.Core
{
	public interface IViewModel : INotifyPropertyChanged
	{
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
