using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login
{
	public class LoginViewModel : DialogViewModelBase, ILoginViewModel
	{
		private IContactsService _contactsService;

		public LoginViewModel(IView view, IContactsService contactsService) : base(view)
		{
			this._contactsService = contactsService ?? throw new ArgumentNullException(nameof(contactsService));
			this.LoginCommand = new RelayCommand(this.Login, this.CanLogin);
			this.Title = "Login to Contacts app";
		}

		public User User { get; private set; }

		public ICommand LoginCommand { get; }

		private string _userName;
		public string UserName
		{
			get => this._userName;
			set => SetProperty(ref this._userName, value);
		}

		private string _password;
		public string Password
		{
			get => this._password;
			set => SetProperty(ref this._password, value);
		}

		private string _errorMessage;
		public string ErrorMessage
		{
			get => this._errorMessage;
			set => SetProperty(ref this._errorMessage, value);
		}




		private async Task Login()
		{
			this.IsBusy = true;
			
			this.ErrorMessage = null;

			this.User = await this._contactsService.Login(this.UserName, this.Password);
			
			if (this.User != null)
				this._view.Close();
			else
				this.ErrorMessage = "Wrong user/pass";

			this.IsBusy = false;
		}

		private bool CanLogin()
		{
			if (string.IsNullOrEmpty(this.UserName) || string.IsNullOrEmpty(this.Password))
				return false;
			return true;
		}


	}
}
