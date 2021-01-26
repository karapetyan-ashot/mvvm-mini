using System;
using System.Collections.Generic;
using System.Text;
using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.ContactEditor;
using EasySoftware.MvvmMini.Samples.Contacts.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;

namespace EasySoftware.MvvmMini.Samples.Contacts.Tests
{
	[TestClass]
	public class ContactEditorTests
	{
		[TestMethod]
		public void CreateContact_ContactCreated()
		{
			// arrange
			var viewAdapter = new Mock<IViewAdapter>();
			var contact = new Contact();
			var expectedContact = new Contact { Name = "Name", Phone = "123", Email = "aa@bb.com" };
			var contactEditor = new ContactEditorViewModel(viewAdapter.Object, contact);
			contactEditor.ShowDialog();

			// act
			contactEditor.Name = "Name";
			contactEditor.Phone = "123";
			contactEditor.Email = "aa@bb.com";
			contactEditor.SaveCommand.Execute(null);

			// assert
			viewAdapter.Verify(x => x.ShowDialog(), Times.Once);
			viewAdapter.Verify(x => x.Close(), Times.Once);
			expectedContact.Should().BeEquivalentTo(contactEditor.ModifiedContact);
		}

		[TestMethod]
		public void CreateContactCanceled_ContactNotCreated()
		{
			// arrange
			var viewAdapter = new Mock<IViewAdapter>();
			var contact = new Contact();			
			var contactEditor = new ContactEditorViewModel(viewAdapter.Object, contact);
			contactEditor.ShowDialog();

			// act
			contactEditor.Name = "Name";
			contactEditor.Phone = "123";
			contactEditor.Email = "aa@bb.com";
			contactEditor.CancelCommand.Execute(null);

			// assert
			viewAdapter.Verify(x => x.ShowDialog(), Times.Once);
			viewAdapter.Verify(x => x.Close(), Times.Once);
			Assert.IsNull(contactEditor.ModifiedContact);
		}

	}
}
