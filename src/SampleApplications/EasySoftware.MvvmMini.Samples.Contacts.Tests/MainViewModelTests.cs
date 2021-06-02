using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.MessageBox;
using EasySoftware.MvvmMini.Samples.Contacts.Factories;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace EasySoftware.MvvmMini.Samples.Contacts.Tests
{
	[TestClass]
	public class MainViewModelTests : ViewModelTestBase
	{
		private Mock<IContactsService> _contactService;
		private Mock<IAppViewModelFactory> _viewModelFactory;
		private Mock<IContactEditorViewModel> _contactEditor = new Mock<IContactEditorViewModel>();
		private Mock<IMessageBoxViewModel> _messageBoxDialog = new Mock<IMessageBoxViewModel>();

		[TestInitialize]
		public override void Init()
		{
			base.Init();

			this._contactService = new Mock<IContactsService>();
			this._contactService.Setup(x => x.GetContacts()).Returns(Task.FromResult<List<ContactModel>>(this.GenerateContacts()));

			this._viewModelFactory = new Mock<IAppViewModelFactory>();
			this._viewModelFactory
				.Setup(x => x.ResolveViewModel<IContactEditorViewModel>(It.IsAny<(string name, object value)>()))
				.Returns(this._contactEditor.Object);
			this._viewModelFactory
				.Setup(x => x.CreateMessageBoxDialog(It.IsAny<String>(), It.IsAny<string>(), MessageBoxButton.YesNo))
				.Returns(this._messageBoxDialog.Object);

		}

		[TestMethod]
		public void MainViewModel_Show_ContactsLoaded()
		{
			// arrange
			IMainViewModel mainViewModel = this.CreateSubject();

			// act
			mainViewModel.Show();

			// assert
			Assert.AreEqual(9, mainViewModel.Contacts.Count);
		}

		[TestMethod]
		public void MainViewModel_CreateContact_ContactCreated()
		{
			// arrange
			ContactModel createdContact = new ContactModel { Id = 10, Email = "aa@bb.com", Name = "name", Phone = "123", Modified = DateTime.Now };
			this._contactEditor.Setup(x => x.DialogResult).Returns(createdContact);

			IMainViewModel mainViewModel = this.CreateSubject();

			// act
			mainViewModel.Show();
			mainViewModel.CreateContactCommand.Execute(null);

			// assert
			Assert.IsTrue(mainViewModel.Contacts.Contains(createdContact));
			Assert.AreEqual(createdContact, mainViewModel.CurrentContact);
			this._viewModelFactory.Verify(x => x.ResolveViewModel<IContactEditorViewModel>(It.IsAny<(string name, object value)>()), Times.Once);
			this._contactEditor.Verify(x => x.ShowDialog(), Times.Once());
		}

		[TestMethod]
		public void MainViewModel_CreateContactCanceled_ContactNotCreated()
		{
			// arrange
			this._contactEditor.Setup(x => x.DialogResult).Returns((ContactModel)null);

			IMainViewModel mainViewModel = this.CreateSubject();

			// act
			mainViewModel.Show();
			mainViewModel.CreateContactCommand.Execute(null);

			// assert			
			Assert.IsNull(mainViewModel.CurrentContact);
			this._viewModelFactory.Verify(x => x.ResolveViewModel<IContactEditorViewModel>(It.IsAny<(string name, object value)>()), Times.Once);
			this._contactEditor.Verify(x => x.ShowDialog(), Times.Once());
		}

		[TestMethod]
		public void MainViewModel_EditContact_ContactModified()
		{
			// arrange
			ContactModel updatedContact = new ContactModel
			{
				Id = 3,
				Name = "UpdatedName",
				Email = "updatedEmail@gmail.com",
				Phone = "11-22-33",
				Modified = new DateTime(2020, 02, 15)
			};
			this._contactEditor.Setup(x => x.DialogResult).Returns(updatedContact);

			IMainViewModel mainViewModel = this.CreateSubject();

			// act
			mainViewModel.Show();
			ContactModel contactToUpdate = mainViewModel.Contacts[2];
			mainViewModel.CurrentContact = contactToUpdate;
			mainViewModel.EditContactCommand.Execute(null);

			// assert
			Assert.IsFalse(mainViewModel.Contacts.Contains(contactToUpdate));
			Assert.IsTrue(mainViewModel.Contacts.Contains(updatedContact));
			Assert.AreEqual(updatedContact, mainViewModel.CurrentContact);
		}

		[TestMethod]
		public void MainViewModel_EditContactCanceled_ContactNotModified()
		{
			// arrange
			this._contactEditor.Setup(x => x.DialogResult).Returns((ContactModel)null);

			IMainViewModel mainViewModel = this.CreateSubject();

			// act
			mainViewModel.Show();
			ContactModel contactToUpdate = mainViewModel.Contacts[2];
			mainViewModel.CurrentContact = contactToUpdate;
			mainViewModel.EditContactCommand.Execute(null);

			// assert
			Assert.IsTrue(mainViewModel.Contacts.Contains(contactToUpdate));
		}

		[TestMethod]
		public void DeleteContact_ContactModified()
		{
			// arrange
			this._messageBoxDialog.Setup(x => x.DialogResult).Returns(MessageBoxResult.Yes);
			IMainViewModel mainViewModel = this.CreateSubject();

			// act
			mainViewModel.Show();
			ContactModel contactToDelete = mainViewModel.Contacts[2];
			mainViewModel.CurrentContact = contactToDelete;
			mainViewModel.DeleteContactCommand.Execute(null);

			// assert
			Assert.IsFalse(mainViewModel.Contacts.Contains(contactToDelete));
		}

		[TestMethod]
		public void DeleteContactCanceled_ContactIsNotModified()
		{
			// arrange
			this._messageBoxDialog.Setup(x => x.DialogResult).Returns(MessageBoxResult.No);
			IMainViewModel mainViewModel = this.CreateSubject();

			// act
			mainViewModel.Show();
			ContactModel contactToDelete = mainViewModel.Contacts[2];
			mainViewModel.CurrentContact = contactToDelete;
			mainViewModel.DeleteContactCommand.Execute(null);

			// assert
			Assert.IsTrue(mainViewModel.Contacts.Contains(contactToDelete));
		}

		private IMainViewModel CreateSubject()
		{
			return new MainViewModel(this._viewAdapter.Object, this._contactService.Object, this._viewModelFactory.Object);
		}

		private List<ContactModel> GenerateContacts()
		{
			List<ContactModel> contacts = new List<ContactModel>();

			for (int i = 1; i < 10; i++)
			{
				contacts.Add(new ContactModel
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
	}}