using EasySoftware.MvvmMini.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace EasySoftware.MvvmMini.Tests
{
	[TestClass]
	public class ViewModelTests
	{
		[TestMethod]
		public void TestMethod1()
		{
			var view = new Mock<IView>();
			IViewModel viewModel = new ViewModelBase(view.Object);
			
		}
	}
}
