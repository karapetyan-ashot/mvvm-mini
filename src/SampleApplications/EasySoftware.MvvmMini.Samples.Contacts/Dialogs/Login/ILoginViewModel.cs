using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login
{
	public interface ILoginViewModel : IDialogViewModel
	{
		User User { get; }
	}
}
