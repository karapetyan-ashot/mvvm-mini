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
			var viewAdapter = new Mock<IViewAdapter>();

			var contactsService = new Mock<IContactsService>();
			contactsService.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<User>(loggedInUser));

			var loginViewModel = new LoginViewModel(viewAdapter.Object, contactsService.Object);
			loginViewModel.ShowDialog();

			// act
			loginViewModel.UserName = "userName";
			loginViewModel.Password = "1";
			loginViewModel.LoginCommand.Execute(null);

			// assert

			viewAdapter.Verify(x => x.Close(), Times.Once);
			contactsService.Verify(x => x.Login(It.Is<string>(p => p == "userName"), It.Is<string>(p => p == "1")), Times.Once);
			
			Assert.AreEqual(loggedInUser, loginViewModel.User);
		}

		[TestMethod]
		public void Login_LoginFailed()
		{
			// arrange
			var viewAdapter = new Mock<IViewAdapter>();

			var contactsService = new Mock<IContactsService>();
			contactsService.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<User>((User)null));

			var loginViewModel = new LoginViewModel(viewAdapter.Object, contactsService.Object);
			loginViewModel.ShowDialog();

			// act
			loginViewModel.UserName = "userName";
			loginViewModel.Password = "2";
			loginViewModel.LoginCommand.Execute(null);

			// assert

			viewAdapter.Verify(x => x.Close(), Times.Never);
			contactsService.Verify(x => x.Login(It.Is<string>(p => p == "userName"), It.Is<string>(p => p == "2")), Times.Once);
			Assert.AreEqual(null, loginViewModel.User);
			Assert.AreEqual("- Wrong user/pass", loginViewModel[""].Trim());
			
		}
	}
}
