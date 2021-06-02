using System;

using EasySoftware.MvvmMini.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace EasySoftware.MvvmMini.Samples.Contacts.Tests
{
	[TestClass]
	public abstract class ViewModelTestBase
	{
		protected	Mock<IViewAdapter> _viewAdapter;

		[TestInitialize]
		public virtual void Init()
		{
			this._viewAdapter = new Mock<IViewAdapter>();
			// to call ViewModel.Loaded
			this._viewAdapter.Setup(x => x.ShowDialog()).Raises(x => x.Loaded += null, EventArgs.Empty);
			this._viewAdapter.Setup(x => x.Show()).Raises(x => x.Loaded += null, EventArgs.Empty);
		}
	}
}