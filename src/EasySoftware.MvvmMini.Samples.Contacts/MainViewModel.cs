using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Contacts.Factories;
using EasySoftware.MvvmMini.Samples.Contacts.Services;
using EasySoftware.MvvmMini.Samples.Contacts.Helpers;

namespace EasySoftware.MvvmMini.Samples.Contacts
{
	public class MainViewModel : WindowViewModelBase, IMainViewModel
	{
		private IContactsService _contactsService;
		private IAppViewModelFactory _viewModelFactory;

		public MainViewModel(IViewAdapter viewAdapter, IContactsService contactsService, IAppViewModelFactory viewModelFactory) : base(viewAdapter)
		{
			this._contactsService = contactsService ?? throw new ArgumentNullException(nameof(contactsService));
			this._viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));

			this.Contacts = new ObservableCollection<Contact>();

			this.CreateContactCommand = new RelayCommand(this.CreateContact);
			this.EditContactCommand = new RelayCommand(this.EditContact, this.CanEditContact);
			this.DeleteContactCommand = new RelayCommand(this.DeleteContact, this.CanDeleteContact);
		}

		public ICommand CreateContactCommand { get; }
		public ICommand EditContactCommand { get; }
		public ICommand DeleteContactCommand { get; }

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

		private async Task CreateContact()
		{
			IContactEditorViewModel contactEditor = this._viewModelFactory.ResolveViewModel<IContactEditorViewModel>(new KeyValuePair<string, object>("contact", new Contact()));
			contactEditor.ShowDialog();
			if (contactEditor.ModifiedContact != null)
			{
				this.IsBusy = true;
				Contact newContact = await this._contactsService.CreateContact(contactEditor.ModifiedContact);
				this.Contacts.Add(newContact);
				this.CurrentContact = newContact;
				this.IsBusy = false;
			}
		}

		private async Task EditContact()
		{
			IContactEditorViewModel contactEditor = this._viewModelFactory.ResolveViewModel<IContactEditorViewModel>(new KeyValuePair<string, object>("contact", this.CurrentContact));
			contactEditor.ShowDialog();
			if (contactEditor.ModifiedContact != null)
			{
				this.IsBusy = true;
				Contact modifiedContact = await this._contactsService.UpdateContact(contactEditor.ModifiedContact);
				this.Contacts.Replace(this.CurrentContact, modifiedContact);
				this.CurrentContact = modifiedContact;
				this.IsBusy = false;
			}
		}

		private bool CanEditContact()
		{
			return this.CurrentContact != null;
		}

		private async Task DeleteContact()
		{
			IMessageBoxViewModel messageBoxDialog = this._viewModelFactory.CreateMessageBoxDialog("Are you sure?", "Confirm deletion", System.Windows.MessageBoxButton.YesNo);
			messageBoxDialog.ShowDialog();
			if (messageBoxDialog.DialogResult == MessageBoxResult.Yes)
			{
				this.IsBusy = true;

				await this._contactsService.DeleteContact(this.CurrentContact);
				this.Contacts.Remove(this.CurrentContact);
				this.CurrentContact = null;

				this.IsBusy = false;
			}
		}

		private bool CanDeleteContact()
		{
			return this.CurrentContact != null;
		}


	}
}
