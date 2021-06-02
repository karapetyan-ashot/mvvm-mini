using System;
using System.Threading.Tasks;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor
{
	public class ContactEditorViewModel : DialogViewModelBase<ContactModel>, IContactEditorViewModel
	{
		private readonly IContactsService _contactService;

		public ContactEditorViewModel(IViewAdapter viewAdapter, IContactsService contactService, ContactModel contact) : base(viewAdapter)
		{
			this._contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
			this.Contact = contact ?? throw new ArgumentNullException(nameof(contact));

			this.Title = this.Contact.Id <= 0 ? "Create contact" : "Edit contact";

			this.SaveCommand = new RelayCommand(this.Save);
		}

		public IRelayCommand SaveCommand { get; }

		public override ContactModel DialogResult { get; protected set; }

		private ContactModel _contact;
		public ContactModel Contact
		{
			get => this._contact;
			set
			{
				if (this._contact != value)
				{
					this._contact = value;
					this._contact.PropertyChanged += (s, e) => { this._contact.ClearErrors(); };
					this.RaisePropertyChanged(nameof(Contact));
				}
			}
		}

		private async Task Save()
		{
			this.Validate();

			if (!this.Contact.HasErrors)
			{
				this.IsBusy = true;

				if (this.Contact.Id <= 0)
					this.Contact = await this._contactService.CreateContact(this.Contact);
				else
					this.Contact = await this._contactService.UpdateContact(this.Contact);

				this.IsBusy = false;

				if (!this.Contact.HasErrors)
				{
					this.DialogResult = this.Contact;
					this.CloseCommand.Execute(null);
				}
			}
		}

		// client side partial validation
		private void Validate()
		{
			this.Contact.ClearErrors();

			if (string.IsNullOrEmpty(this.Contact.Name))
				this.Contact.AddError(nameof(Contact.Name), "name is required");
			if (string.IsNullOrEmpty(this.Contact.Phone))
				this.Contact.AddError(nameof(Contact.Phone), "phone is required");
			if (string.IsNullOrEmpty(this.Contact.Email))
				this.Contact.AddError(nameof(Contact.Email), "Email is required");
		}
	}
}
