using System.Threading.Tasks;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace EasySoftware.MvvmMini.Samples.Contacts.Tests
{
	[TestClass]

	public class LoginViewModelTests
	{

		[TestMethod]
		public void Login_LoginSuccess()
		{
			// arrange
			var loggedInUser = new User { Id = 1, Name = "name", UserName = "userName", Password = "1" };
			var view = new Mock<IView>();

			var contactsService = new Mock<IContactsService>();
			contactsService.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<User>(loggedInUser));

			var loginViewModel = new LoginViewModel(view.Object, contactsService.Object);
			loginViewModel.ShowDialog();

			// act
			loginViewModel.UserName = "userName";
			loginViewModel.Password = "1";
			loginViewModel.LoginCommand.Execute(null);

			// assert

			view.Verify(x => x.Close(), Times.Once);
			contactsService.Verify(x => x.Login(It.Is<string>(p => p == "userName"), It.Is<string>(p => p == "1")), Times.Once);
			
			Assert.AreEqual(loggedInUser, loginViewModel.User);
		}
	}
}
