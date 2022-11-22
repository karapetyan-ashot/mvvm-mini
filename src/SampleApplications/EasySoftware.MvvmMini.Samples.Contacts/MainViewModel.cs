using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Contacts.Factories;
using EasySoftware.MvvmMini.Samples.Contacts.Helpers;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

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

			this.Contacts = new ObservableCollection<ContactModel>();

			this.CreateContactCommand = new RelayCommand(this.CreateContact);
			this.EditContactCommand = new RelayCommand(this.EditContact, this.CanEditContact);
			this.DeleteContactCommand = new RelayCommand(this.DeleteContact, this.CanDeleteContact);
		}

		public IRelayCommand CreateContactCommand { get; }
		public IRelayCommand EditContactCommand { get; }
		public IRelayCommand DeleteContactCommand { get; }

		private ObservableCollection<ContactModel> _contacts;
		public ObservableCollection<ContactModel> Contacts
		{
			get => this._contacts;
			set => SetProperty(ref this._contacts, value);
		}

		private ContactModel _currentContact;
		public ContactModel CurrentContact
		{
			get => this._currentContact;
			set => SetProperty(ref this._currentContact, value);
		}

		protected override async Task Loaded()
		{
			this.IsBusy = true;

			List<ContactModel> contacts = await this._contactsService.GetContacts();
			this.Contacts = new ObservableCollection<ContactModel>(contacts);

			this.IsBusy = false;
		}

		private Task CreateContact()
		{
			IContactEditorViewModel contactEditor = this._viewModelFactory.CreateContactEditorDialog(new ContactModel());
			contactEditor.ShowDialog();
			if (contactEditor.DialogResult != null)
			{	
				this.Contacts.Add(contactEditor.DialogResult);
				this.CurrentContact = contactEditor.DialogResult;
			}

			return Task.CompletedTask;
		}

		private Task EditContact()
		{
			IContactEditorViewModel contactEditor = this._viewModelFactory.CreateContactEditorDialog(this.CurrentContact);
			contactEditor.ShowDialog();
			if (contactEditor.DialogResult != null)
			{	
				this.Contacts.Replace(this.CurrentContact, contactEditor.DialogResult);
				this.CurrentContact = contactEditor.DialogResult;
			}
			
			return Task.CompletedTask;
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