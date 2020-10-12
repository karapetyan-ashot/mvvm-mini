using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login
{
	public class LoginViewModel : DialogViewModelBase, IDialogViewModel
	{
		public LoginViewModel(IView view) : base(view)
		{
			this.LoginCommand = new RelayCommand(this.Login, this.CanLogin);
		}

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

		private Task Login()
		{
			
		}

		private bool CanLogin()
		{
			if (string.IsNullOrEmpty(this.UserName) || string.IsNullOrEmpty(this.Password))
				return false;
			return true;
		}


	}
}
