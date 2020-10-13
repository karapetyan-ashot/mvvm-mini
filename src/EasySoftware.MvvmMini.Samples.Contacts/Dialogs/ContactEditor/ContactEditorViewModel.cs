using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor
{
	public class ContactEditorViewModel : DialogViewModelBase, IContactEditor
	{
		private Contact _contact;

		public ContactEditorViewModel(IView view, Contact contact) : base(view)
		{
			this._contact = contact ?? throw new ArgumentNullException(nameof(contact));

			this.Name = this._contact.Name;
			this.Sex = this._contact.Sex;
			this.Email = this._contact.Email;

			this.Title = contact.Id <= 0 ? "Create contact" : "Edit contact";

			this.SaveCommand = new RelayCommand(this.Save);
			this.CancelCommand = new RelayCommand(this.Cancel);
		}

		public ICommand SaveCommand { get; }
		public ICommand CancelCommand { get; }

		public Contact ModifiedContact { get; private set; }

		private string _name;
		public string Name
		{
			get => this._name;
			set => SetProperty(ref this._name, value);
		}

		private bool _sex;
		public bool Sex
		{
			get => this._sex;
			set => SetProperty(ref this._sex, value);
		}

		private string _email;
		public string Email
		{
			get => this._email;
			set => SetProperty(ref this._email, value);
		}

		private Task Save()
		{
			this.Validate();
			if(!this.HasErrors)
			{
				this._contact.Name = this.Name;
				this._contact.Sex = this.Sex;
				this._contact.Email = this.Email;
				this.ModifiedContact = this._contact;
				this._view.Close();

			}
			return Task.CompletedTask;
		}

		private Task Cancel()
		{
			this.ModifiedContact = null;
			this._view.Close();
			return Task.CompletedTask;
		}

		private void Validate()
		{
			this.ClearErrors();
			if (string.IsNullOrEmpty(this.Name))
				this.AddError(nameof(Name), "name is required");
			if(string.IsNullOrEmpty(this.Email))
				this.AddError(nameof(Email), "Email is required");
			else
			{
				if (!this.Email.Contains("@"))
					this.AddError(nameof(Email), "Not valid email");
			}
		}



	}
}
