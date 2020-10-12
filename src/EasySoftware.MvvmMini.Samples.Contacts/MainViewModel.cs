using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

namespace EasySoftware.MvvmMini.Samples.Contacts
{
	public class MainViewModel : WindowViewModelBase
	{
		private IContactsService _contactsService;

		public MainViewModel(IView view, IContactsService contactsService) : base(view)
		{
			this._contactsService = contactsService ?? throw new ArgumentNullException(nameof(contactsService));
		}

		private ObservableCollection<Contact> _contacts;
		public ObservableCollection<Contact> Contacts
		{
			get => this._contacts;
			set => SetProperty(ref this._contacts, value);
		}

		private Contact _currentContact;
		public Contact CurrentContact
		{
			get => this._currentContact;
			set => SetProperty(ref this._currentContact, value);
		}

		protected override async Task Loaded()
		{
			this.IsBusy = true;

			List<Contact> contacts = await this._contactsService.GetContacts();
			this.Contacts = new ObservableCollection<Contact>(contacts);

			this.IsBusy = false;
		}
	}
}
