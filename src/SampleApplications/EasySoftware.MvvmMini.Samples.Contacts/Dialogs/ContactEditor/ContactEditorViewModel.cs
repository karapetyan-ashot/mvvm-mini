using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor
{
	public class ContactEditorViewModel : DialogViewModelBase, IContactEditorViewModel
	{
		private Contact _contactToEdit;

		public ContactEditorViewModel(IViewAdapter viewAdapter, Contact contact) : base(viewAdapter)
		{
			if (contact == null)
				throw new ArgumentNullException(nameof(contact));
			this._contactToEdit = new Contact { Id = contact.Id, Modified = contact.Modified };

			this.Name = contact.Name;
			this.Phone = contact.Phone;
			this.Email = contact.Email;

			this.Title = contact.Id <= 0 ? "Create contact" : "Edit contact";

			this.SaveCommand = new RelayCommand(this.Save);
			this.CancelCommand = new RelayCommand(this.Cancel);
		}

		public IRelayCommand SaveCommand { get; }
		public IRelayCommand CancelCommand { get; }

		public Contact ModifiedContact { get; private set; }

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

		private Task Save()
		{
			this.Validate();
			if (!this.HasErrors)
			{
				this._contactToEdit.Name = this.Name;
				this._contactToEdit.Phone = this.Phone;
				this._contactToEdit.Email = this.Email;
				this.ModifiedContact = this._contactToEdit;
				this._viewAdapter.Close();
			}
			return Task.CompletedTask;
		}

		private Task Cancel()
		{
			this.ModifiedContact = null;
			this._viewAdapter.Close();
			return Task.CompletedTask;
		}

		private void Validate()
		{
			this.ClearErrors();
			if (string.IsNullOrEmpty(this.Name))
				this.AddError(nameof(Name), "name is required");
			if (string.IsNullOrEmpty(this.Email))
				this.AddError(nameof(Email), "Email is required");
			else
			{
				if (!this.Email.Contains("@"))
					this.AddError(nameof(Email), "Not valid email");
			}
		}
	}
}
