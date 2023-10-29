using System;
using System.Threading.Tasks;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login
{
	public class LoginViewModel : DialogViewModelBase<UserModel>, ILoginViewModel
	{
		private IContactsService _contactsService;

		public LoginViewModel(IViewAdapter viewAdapter, IContactsService contactsService) : base(viewAdapter)
		{
			this._contactsService = contactsService ?? throw new ArgumentNullException(nameof(contactsService));
			this.Title = "Login to Contacts app";

			this.LoginCommand = new RelayCommand(this.Login, this.CanLogin);
		}

		public IRelayCommand LoginCommand { get; }

		public override UserModel DialogResult { get; protected set; }

		private string _userName;
		public string UserName
		{
			get => this._userName;
			set
			{
				if (SetProperty(ref this._userName, value))
				{
					this.ClearErrors(string.Empty);
					this.ClearErrors(nameof(UserName));
					if (string.IsNullOrEmpty(this._userName))
						this.AddError(nameof(UserName), "username is required");
				}
			}
		}

		private string _password;
		public string Password
		{
			get => this._password;
			set
			{
				if (SetProperty(ref this._password, value))
				{
					this.ClearErrors(string.Empty);
					this.ClearErrors(nameof(Password));
					if (string.IsNullOrEmpty(this._password))
						this.AddError(nameof(Password), "password is required");
				}
			}
		}

        protected override Task Loaded()
        {
#if DEBUG
			UserName = "username";
			Password = "1";
#endif
			return base.Loaded();
        }


        private async Task Login()
		{
			this.IsBusy = true;

			var user = await this._contactsService.Login(this.UserName, this.Password);

			if (!user.HasErrors)
			{
				this.DialogResult = user;
				this.CloseCommand.Execute(null);
			}
			else
			{
				this.CloneErrors(user);
			}

			this.IsBusy = false;
		}

		private bool CanLogin()
		{
			return !this.HasErrors && !string.IsNullOrEmpty(this.UserName) && !string.IsNullOrEmpty(this.Password);
		}
	}
}