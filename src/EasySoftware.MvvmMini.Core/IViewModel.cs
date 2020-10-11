using System.ComponentModel;

namespace EasySoftware.MvvmMini.Core
{
	public interface IViewModel : INotifyPropertyChanged
	{
		string Title { get; set; }
		bool IsBusy { get; }
		object View { get; }
	}
}
