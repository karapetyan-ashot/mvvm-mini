using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login
{
	public interface ILoginViewModel : IDialogViewModel<UserModel>
	{
		IRelayCommand LoginCommand { get; }

		public string UserName { get; set; }
		public string Password { get; set; }
	}
}