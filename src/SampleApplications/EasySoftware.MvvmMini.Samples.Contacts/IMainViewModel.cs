using System.Collections.ObjectModel;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts
{
	public interface IMainViewModel : IWindowViewModel
	{
		IRelayCommand CreateContactCommand { get; }
		IRelayCommand EditContactCommand { get; }
		IRelayCommand DeleteContactCommand { get; }

		ObservableCollection<ContactModel> Contacts { get; }
		ContactModel CurrentContact { get; set; }
	}
}