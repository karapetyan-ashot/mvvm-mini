using System;
using System.Collections.Generic;

using EasySoftware.Abstractions;
using EasySoftware.MvvmMini.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace EasySoftware.MvvmMini.Samples.Contacts.Tests
{
    [TestClass]
    public abstract class ViewModelTestBase
    {
        protected Mock<IViewAdapter> _viewAdapter;

        [TestInitialize]
        public virtual void Init()
        {
            this._viewAdapter = new Mock<IViewAdapter>();
            // to call ViewModel.Loaded
            this._viewAdapter.Setup(x => x.ShowDialog()).Raises(x => x.Loaded += null, EventArgs.Empty);
            this._viewAdapter.Setup(x => x.Show()).Raises(x => x.Loaded += null, EventArgs.Empty);
        }
    }

    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void TestCloneErrors()
        {
            TestModel tm = new TestModel();
            LocalModel lm = new LocalModel();
            tm.CloneErrors(lm);
        }
    }

    public class TestModel : ModelBase
    {

    }

    public class LocalModel: IErrorContainer
    {
        public bool HasErrors => true;

        public IReadOnlyDictionary<string, IEnumerable<string>> Errors { get; set; }

        public void AddError(string errorMessage)
        {
            
        }

        public void AddError(string propName, string errorMessage)
        {
           
        }

        public void ClearErrors()
        {
           
        }

        public void ClearErrors(string propName)
        {
            
        }

        public void CloneErrors(IErrorContainer other)
        {
            
        }
    }
}