using System.Threading.Tasks;

using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace EasySoftware.MvvmMini.Samples.Contacts.Tests
{
	[TestClass]
	public class ContactEditorTests : ViewModelTestBase
	{
		private Mock<IContactsService> _contactService;

		[TestInitialize]
		public override void Init()
		{
			base.Init();

			this._contactService = new Mock<IContactsService>();
		}

		[TestMethod]
		public void CreateContact_Save_ContactCreated()
		{
			// arrange
			ContactModel serviceResult = new ContactModel { Id = -1, Name = "Name", Phone = "123", Email = "aa@bb.com" };
			this._contactService.Setup(x => x.CreateContact(It.IsAny<ContactModel>())).Returns(Task.FromResult<ContactModel>(serviceResult));
			IContactEditorViewModel contactEditor = this.CreateSubject(new ContactModel());

			// act
			contactEditor.ShowDialog();
			contactEditor.Contact.Name = "Name";
			contactEditor.Contact.Phone = "123";
			contactEditor.Contact.Email = "aa@bb.com";
			contactEditor.SaveCommand.Execute(null);

			// assert
			ContactModel expected = new ContactModel { Id = -1, Name = "Name", Phone = "123", Email = "aa@bb.com" };
			expected.Should().BeEquivalentTo(contactEditor.DialogResult);
		}

		[TestMethod]
		public void CreateContact_Cancel_ContactNotCreated()
		{
			// arrange
			IContactEditorViewModel contactEditor = this.CreateSubject(new ContactModel());

			// act
			contactEditor.ShowDialog();
			contactEditor.Contact.Name = "Name";
			contactEditor.Contact.Phone = "123";
			contactEditor.Contact.Email = "aa@bb.com";
			contactEditor.CloseCommand.Execute(null);

			// assert
			this._viewAdapter.Verify(x => x.ShowDialog(), Times.Once);
			this._viewAdapter.Verify(x => x.Close(), Times.Once);
			Assert.IsNull(contactEditor.DialogResult);
		}

		[TestMethod]
		public void UpdateContact_Save_ContactUpdated()
		{
			// arrange
			ContactModel serviceResult = new ContactModel { Id = 1, Name = "newName", Phone = "123", Email = "aa@bb.com" };
			this._contactService.Setup(x => x.UpdateContact(It.IsAny<ContactModel>())).Returns(Task.FromResult<ContactModel>(serviceResult));
			ContactModel contactToUpdate = new ContactModel { Id = 1, Name = "name", Phone = "123", Email = "aa@bb.com" };
			IContactEditorViewModel contactEditor = this.CreateSubject(contactToUpdate);

			// act
			contactEditor.ShowDialog();
			contactEditor.Contact.Name = "newName";
			contactEditor.SaveCommand.Execute(null);

			// assert
			Assert.IsNotNull(contactEditor.DialogResult);
			ContactModel expected = new ContactModel { Id = 1, Name = "newName", Phone = "123", Email = "aa@bb.com" };
			expected.Should().BeEquivalentTo(contactEditor.DialogResult);
		}

		[TestMethod]
		public void UpdateContact_Save_Validated()
		{
			// arrange
			ContactModel contactToUpdate = new ContactModel { Id = 1, Name = "name", Phone = "123", Email = "aa@bb.com" };
			IContactEditorViewModel contactEditor = this.CreateSubject(contactToUpdate);

			// act
			contactEditor.ShowDialog();
			contactEditor.Contact.Name = string.Empty;
			contactEditor.SaveCommand.Execute(null);

			// assert
			Assert.IsTrue(contactEditor.Contact.HasErrors);
		}

		private IContactEditorViewModel CreateSubject(ContactModel contact)
		{
			return new ContactEditorViewModel(this._viewAdapter.Object, this._contactService.Object, contact);
		}
	}
}
