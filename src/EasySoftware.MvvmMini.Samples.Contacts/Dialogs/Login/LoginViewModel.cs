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

		public LoginViewModel(IViewAdapter viewAdapter, IContactsService contactsService) : base(viewAdapter)
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

		private async Task Login()
		{
			this.Validate();

			if (!this.HasErrors)
			{
				this.IsBusy = true;
								
				this.User = await this._contactsService.Login(this.UserName, this.Password);

				if (this.User != null)
					this._viewAdapter.Close();
				else
					this.AddError("Wrong user/pass");

				this.IsBusy = false;
			}
			
		}

		private bool CanLogin()
		{
			if (string.IsNullOrEmpty(this.UserName) || string.IsNullOrEmpty(this.Password))
				return false;
			return true;
		}


		private void Validate()
		{
			this.ClearErrors();
			
			if (string.IsNullOrEmpty(this.UserName))
				this.AddError(nameof(UserName), "username is required");
			if(this.UserName != null && this.UserName.Length < 2)
				this.AddError(nameof(UserName), "username lenght must be >= 2");
			if (string.IsNullOrEmpty(this.Password))
				this.AddError(nameof(Password), "password is required");

			this.RaiseErrorsChanged(string.Empty);
		}

	}
}
