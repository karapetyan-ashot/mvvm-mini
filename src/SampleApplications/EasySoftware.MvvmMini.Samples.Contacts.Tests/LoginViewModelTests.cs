using System.Threading.Tasks;

using EasySoftware.Abstractions;
using EasySoftware.MvvmMini.Samples.Contacts.Dialogs.Login;
using EasySoftware.MvvmMini.Samples.Contacts.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace EasySoftware.MvvmMini.Samples.Contacts.Tests
{
	[TestClass]
	public class LoginViewModelTests : ViewModelTestBase
	{
		private Mock<IContactsService> _contactService;

		[TestInitialize]
		public override void Init()
		{
			base.Init();

			this._contactService = new Mock<IContactsService>();
		}

		[TestMethod]
		public void Login_Login_Success()
		{
			// arrange
			var loggedInUser = new UserModel { Id = 1, Name = "name", UserName = "username", Password = "1" };
			this._contactService.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<UserModel>(loggedInUser));

			ILoginViewModel loginViewModel = this.CreateSubject();

			// act
			loginViewModel.ShowDialog();
			loginViewModel.UserName = "username";
			loginViewModel.Password = "1";
			loginViewModel.LoginCommand.Execute(null);

			// assert
			this._contactService.Verify(x => x.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			Assert.IsNotNull(loginViewModel.DialogResult);
			Assert.AreEqual("username", loginViewModel.DialogResult.UserName);
			Assert.AreEqual("1", loginViewModel.DialogResult.Password);
		}


		[TestMethod]
		public void Login_Login_Failed()
		{
			// arrange
			var loggedInUser = new UserModel();
			loggedInUser.AddError("invalid username or password");
			this._contactService.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<UserModel>(loggedInUser));

			ILoginViewModel loginViewModel = this.CreateSubject();

			// act
			loginViewModel.ShowDialog();
			loginViewModel.UserName = "username";
			loginViewModel.Password = "2";
			loginViewModel.LoginCommand.Execute(null);

			// assert
			Assert.IsNull(loginViewModel.DialogResult);
			this._contactService.Verify(x => x.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
		}

		[TestMethod]
		public void Login_Login_Validated()
		{
			// arrange
			ILoginViewModel loginViewModel = this.CreateSubject();

			// act
			loginViewModel.ShowDialog();
			loginViewModel.UserName = "";
			loginViewModel.LoginCommand.Execute(null);

			// assert
			Assert.IsTrue(((IErrorContainer)loginViewModel).HasErrors);
		}

		private ILoginViewModel CreateSubject()
		{
			return new LoginViewModel(this._viewAdapter.Object, this._contactService.Object);
		}
	}
}