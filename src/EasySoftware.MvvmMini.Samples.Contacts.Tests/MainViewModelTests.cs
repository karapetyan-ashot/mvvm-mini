using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Contacts.Factories;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace EasySoftware.MvvmMini.Samples.Contacts.Tests
{
	[TestClass]	
	public class MainViewModelTests
	{
		[TestMethod]
		public void LoadContacts_ContactsLoaded()
		{
			// arrange
			var viewAdapter = new Mock<IViewAdapter>();
			var contactsService = new Mock<IContactsService>();
			contactsService.Setup(x => x.GetContacts()).Returns(Task.FromResult<List<Contact>>(this.GenerateContacts()));
			var vmFactory = new Mock<IViewModelFactory>();
			var mainViewModel = new MainViewModel(viewAdapter.Object, contactsService.Object, vmFactory.Object);

			// act
			viewAdapter.Raise(x => x.Loaded += null, new EventArgs());

			// assert
			Assert.AreEqual(9, mainViewModel.Contacts.Count);
		}

		[TestMethod]
		public void CreateContact_ContactCreated()
		{
			// arrange
			var viewAdapter = new Mock<IViewAdapter>();
			var newContact = new Contact();
			var editedByDialogContact = new Contact { Email = "aa@bb.com", Name = "name", Phone = "123" };
			var updatedBuServidceContact = new Contact { Id = 10, Email = "aa@bb.com", Name = "name", Phone = "123", Modified = DateTime.Now };

			var contactEditor = new Mock<IContactEditorViewModel>();
			contactEditor.Setup(x => x.ModifiedContact).Returns(editedByDialogContact);

			var contactsService = new Mock<IContactsService>();
			contactsService.Setup(x => x.GetContacts()).Returns(Task.FromResult<List<Contact>>(this.GenerateContacts()));
			contactsService.Setup(x => x.CreateContact(It.IsAny<Contact>())).Returns(Task.FromResult<Contact>(updatedBuServidceContact));

			var vmFactory = new Mock<IViewModelFactory>();
			vmFactory.Setup(x => x.CreateContactEditorDialog(It.IsAny<Contact>())).Returns(contactEditor.Object);

			var mainViewModel = new MainViewModel(viewAdapter.Object, contactsService.Object, vmFactory.Object);
			viewAdapter.Raise(x => x.Loaded += null, new EventArgs());

			// act
			mainViewModel.CreateContactCommand.Execute(null);

			// assert			
			vmFactory.Verify(x => x.CreateContactEditorDialog(It.IsAny<Contact>()), Times.Once);
			contactEditor.Verify(x => x.ShowDialog(), Times.Once());
			contactsService.Verify(x => x.CreateContact(editedByDialogContact), Times.Once);
			Assert.AreSame(updatedBuServidceContact, mainViewModel.CurrentContact);
			Assert.AreEqual(9, mainViewModel.Contacts.IndexOf(updatedBuServidceContact));
			Assert.IsFalse(mainViewModel.Contacts.Any(x => x == newContact));
		}

		[TestMethod]
		public void CreateContactCanceled_ContactIsNotCreated()
		{
			// arrange
			var viewAdapter = new Mock<IViewAdapter>();
			var newContact = new Contact();

			var contactEditor = new Mock<IContactEditorViewModel>();
			contactEditor.Setup(x => x.ModifiedContact).Returns((Contact)null);

			var contactsService = new Mock<IContactsService>();
			contactsService.Setup(x => x.GetContacts()).Returns(Task.FromResult<List<Contact>>(this.GenerateContacts()));

			var vmFactory = new Mock<IViewModelFactory>();
			vmFactory.Setup(x => x.CreateContactEditorDialog(It.IsAny<Contact>())).Returns(contactEditor.Object);

			var mainViewModel = new MainViewModel(viewAdapter.Object, contactsService.Object, vmFactory.Object);
			viewAdapter.Raise(x => x.Loaded += null, new EventArgs());

			// act
			mainViewModel.CreateContactCommand.Execute(null);

			// assert			
			vmFactory.Verify(x => x.CreateContactEditorDialog(It.IsAny<Contact>()), Times.Once);
			contactEditor.Verify(x => x.ShowDialog(), Times.Once());
			contactsService.Verify(x => x.CreateContact(It.IsAny<Contact>()), Times.Never);
			Assert.IsNull(mainViewModel.CurrentContact);
			Assert.AreEqual(9, mainViewModel.Contacts.Count);
		}

		[TestMethod]
		public void EditContact_ContactModified()
		{
			// arrange
			var contacts = this.GenerateContacts();
			var contactToEdit = contacts[2];
			var editedByDialogContact = new Contact { Id = contactToEdit.Id, Email = "aa@bb.com", Name = "name", Phone = "123", Modified = new DateTime(2020, 01, 15) };
			var updatedBuServidceContact = new Contact { Id = contactToEdit.Id, Email = "aa@bb.com", Name = "name", Phone = "123", Modified = DateTime.Now };

			var viewAdapter = new Mock<IViewAdapter>();
			var contactEditor = new Mock<IContactEditorViewModel>();
			contactEditor.Setup(x => x.ModifiedContact).Returns(editedByDialogContact);

			var contactsService = new Mock<IContactsService>();
			contactsService.Setup(x => x.GetContacts()).Returns(Task.FromResult<List<Contact>>(contacts));
			contactsService.Setup(x => x.UpdateContact(It.IsAny<Contact>())).Returns(Task.FromResult<Contact>(updatedBuServidceContact));

			var vmFactory = new Mock<IViewModelFactory>();
			vmFactory.Setup(x => x.CreateContactEditorDialog(It.IsAny<Contact>())).Returns(contactEditor.Object);

			var mainViewModel = new MainViewModel(viewAdapter.Object, contactsService.Object, vmFactory.Object);
			viewAdapter.Raise(x => x.Loaded += null, new EventArgs());
			mainViewModel.CurrentContact = contactToEdit;

			// act
			mainViewModel.EditContactCommand.Execute(null);

			// assert
			vmFactory.Verify(x => x.CreateContactEditorDialog(contactToEdit), Times.Once);
			contactEditor.Verify(x => x.ShowDialog(), Times.Once());
			contactsService.Verify(x => x.UpdateContact(editedByDialogContact), Times.Once());
			Assert.AreSame(updatedBuServidceContact, mainViewModel.CurrentContact);
			Assert.AreEqual(2, mainViewModel.Contacts.IndexOf(updatedBuServidceContact));
			Assert.IsFalse(mainViewModel.Contacts.Any(x => x == contactToEdit));
		}

		[TestMethod]
		public void EditContactCanceled_ContactIsNotModified()
		{
			// arrange
			var contacts = this.GenerateContacts();
			var contactToEdit = contacts[2];

			var viewAdapter = new Mock<IViewAdapter>();
			var contactEditor = new Mock<IContactEditorViewModel>();
			contactEditor.Setup(x => x.ModifiedContact).Returns((Contact)null);

			var contactsService = new Mock<IContactsService>();
			contactsService.Setup(x => x.GetContacts()).Returns(Task.FromResult<List<Contact>>(contacts));

			var vmFactory = new Mock<IViewModelFactory>();
			vmFactory.Setup(x => x.CreateContactEditorDialog(It.IsAny<Contact>())).Returns(contactEditor.Object);

			var mainViewModel = new MainViewModel(viewAdapter.Object, contactsService.Object, vmFactory.Object);
			viewAdapter.Raise(x => x.Loaded += null, new EventArgs());
			mainViewModel.CurrentContact = contactToEdit;

			// act
			mainViewModel.EditContactCommand.Execute(null);

			// assert
			vmFactory.Verify(x => x.CreateContactEditorDialog(contactToEdit), Times.Once);
			contactEditor.Verify(x => x.ShowDialog(), Times.Once());
			contactsService.Verify(x => x.UpdateContact(It.IsAny<Contact>()), Times.Never);
			Assert.AreSame(contactToEdit, mainViewModel.CurrentContact);
			Assert.AreEqual(2, mainViewModel.Contacts.IndexOf(contactToEdit));
		}

		[TestMethod]
		public void DeleteContact_ContactModified()
		{
			// arrange
			var contacts = this.GenerateContacts();
			var contactToDelete = contacts[2];
			var viewAdapter = new Mock<IViewAdapter>();
			var messageBoxDialog = new Mock<IMessageBoxViewModel>();
			messageBoxDialog.Setup(x => x.DialogResult).Returns(MessageBoxResult.Yes);

			var contactsService = new Mock<IContactsService>();
			contactsService.Setup(x => x.GetContacts()).Returns(Task.FromResult<List<Contact>>(contacts));
			contactsService.Setup(x => x.DeleteContact(It.IsAny<Contact>())).Returns(Task.CompletedTask);

			var vmFactory = new Mock<IViewModelFactory>();
			vmFactory.Setup(x => x.CreateMessageBoxDialog(It.IsAny<String>(), It.IsAny<string>(), MessageBoxButton.YesNo)).Returns(messageBoxDialog.Object);

			var mainViewModel = new MainViewModel(viewAdapter.Object, contactsService.Object, vmFactory.Object);
			viewAdapter.Raise(x => x.Loaded += null, new EventArgs());
			mainViewModel.CurrentContact = contactToDelete;

			// act
			mainViewModel.DeleteContactCommand.Execute(null);

			// assert
			vmFactory.Verify(x => x.CreateMessageBoxDialog(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButton.YesNo), Times.Once);
			messageBoxDialog.Verify(x => x.ShowDialog(), Times.Once());
			contactsService.Verify(x => x.DeleteContact(contactToDelete), Times.Once());
			Assert.IsNull(mainViewModel.CurrentContact);
			Assert.IsFalse(mainViewModel.Contacts.Any(x => x == contactToDelete));
		}

		[TestMethod]
		public void DeleteContactCanceled_ContactIsNotModified()
		{
			// arrange
			var contacts = this.GenerateContacts();
			var contactToDelete = contacts[2];
			var viewAdapter = new Mock<IViewAdapter>();
			var messageBoxDialog = new Mock<IMessageBoxViewModel>();
			messageBoxDialog.Setup(x => x.DialogResult).Returns(MessageBoxResult.No);

			var contactsService = new Mock<IContactsService>();
			contactsService.Setup(x => x.GetContacts()).Returns(Task.FromResult<List<Contact>>(contacts));

			var vmFactory = new Mock<IViewModelFactory>();
			vmFactory.Setup(x => x.CreateMessageBoxDialog(It.IsAny<String>(), It.IsAny<string>(), MessageBoxButton.YesNo)).Returns(messageBoxDialog.Object);

			var mainViewModel = new MainViewModel(viewAdapter.Object, contactsService.Object, vmFactory.Object);
			viewAdapter.Raise(x => x.Loaded += null, new EventArgs());
			mainViewModel.CurrentContact = contactToDelete;

			// act
			mainViewModel.DeleteContactCommand.Execute(null);

			// assert
			vmFactory.Verify(x => x.CreateMessageBoxDialog(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButton.YesNo), Times.Once);
			messageBoxDialog.Verify(x => x.ShowDialog(), Times.Once);
			contactsService.Verify(x => x.DeleteContact(contactToDelete), Times.Never());
			Assert.AreEqual(contactToDelete, mainViewModel.CurrentContact);
			Assert.IsTrue(mainViewModel.Contacts.Any(x => x == contactToDelete));
		}

		private List<Contact> GenerateContacts()
		{
			List<Contact> contacts = new List<Contact>();

			for (int i = 1; i < 10; i++)
			{
				contacts.Add(new Contact
				{
					Id = i,
					Name = $"Contact {i}",
					Email = $"email_{i}@gmail.com",
					Phone = $"33-44-{i}",
					Modified = new DateTime(2020, 01, 15)
				});
			}

			return contacts;
		}
	}
}
