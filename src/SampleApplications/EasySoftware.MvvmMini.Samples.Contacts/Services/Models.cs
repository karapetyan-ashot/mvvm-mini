using System;

namespace EasySoftware.MvvmMini.Samples.Contacts.Services
{
	public class UserModel : ModelBase
	{
		private int _id;
		public int Id
		{
			get => this._id;
			set => SetProperty(ref this._id, value);
		}

		private string _name;
		public string Name
		{
			get => this._name;
			set => SetProperty(ref this._name, value);
		}

		private string _userName;
		public string UserName
		{
			get => this._userName;
			set => SetProperty(ref this._userName, value);
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set
			{
				if (_password != value)
				{
					_password = value;
					RaisePropertyChanged("Password");
				}
			}
		}
	}

	public class ContactModel : ModelBase
	{
		private int _id;
		public int Id
		{
			get => this._id;
			set => SetProperty(ref this._id, value);
		}

		private string _name;
		public string Name
		{
			get => this._name;
			set => SetProperty(ref this._name, value);
		}

		private string _phone;
		public string Phone
		{
			get => this._phone;
			set => SetProperty(ref this._phone, value);
		}

		private string _email;
		public string Email
		{
			get => this._email;
			set => SetProperty(ref this._email, value);
		}

		private DateTime _modified;
		public DateTime Modified
		{
			get => this._modified;
			set => SetProperty(ref this._modified, value);
		}

		public ContactModel Clone()
		{
			ContactModel clone = new ContactModel
			{
				Id = this.Id,
				Name = this.Name,
				Phone = this.Phone,
				Email = this.Email,
				Modified = this.Modified,
			};

			return clone;
		}
	}
}
