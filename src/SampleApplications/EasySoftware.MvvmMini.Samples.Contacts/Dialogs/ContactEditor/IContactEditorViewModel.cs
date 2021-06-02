using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor
{
	public interface IContactEditorViewModel : IDialogViewModel<ContactModel>
	{
		IRelayCommand SaveCommand { get; }
		ContactModel Contact { get; }
	}
}