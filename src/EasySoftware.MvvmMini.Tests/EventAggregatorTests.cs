using EasySoftware.MvvmMini.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace EasySoftware.MvvmMini.Tests
{
	[TestClass]
	public class EventAggregatorTests
	{
		public interface ISubscriber
		{
			void OnInt(int val);
			void OnString(string val);
		}

		[TestMethod]
		public void Publish_Subscribe()
		{
			// arrange
			Mock<ISubscriber> subsriberMock = new Mock<ISubscriber>();			
			IEventAggregator eventAggregator = new EventAggregator();
			eventAggregator.Subscribe<int>(subsriberMock.Object.OnInt);

			// act
			eventAggregator.Publish<int>(3);

			// assert
			subsriberMock.Verify(x => x.OnInt(3), Times.Once);
		}

		[TestMethod]
		public void Publish_Subscribe2Times()
		{
			// arrange
			Mock<ISubscriber> subsriberMock1 = new Mock<ISubscriber>();
			Mock<ISubscriber> subsriberMock2 = new Mock<ISubscriber>();
			IEventAggregator eventAggregator = new EventAggregator();
			eventAggregator.Subscribe<int>(subsriberMock1.Object.OnInt);
			eventAggregator.Subscribe<int>(subsriberMock2.Object.OnInt);

			// act
			eventAggregator.Publish<int>(3);

			// assert
			subsriberMock1.Verify(x => x.OnInt(3), Times.Once);
			subsriberMock2.Verify(x => x.OnInt(3), Times.Once);
		}

		[TestMethod]
		public void NamedPublish_Subscribe()
		{
			// arrange
			string key = "key";
			Mock<ISubscriber> subsriberMock1 = new Mock<ISubscriber>();
			Mock<ISubscriber> subsriberMock2 = new Mock<ISubscriber>();
			IEventAggregator eventAggregator = new EventAggregator();
			eventAggregator.Subscribe<int>(subsriberMock1.Object.OnInt, key);
			eventAggregator.Subscribe<int>(subsriberMock2.Object.OnInt, key);

			// act
			eventAggregator.Publish<int>(3, key);

			// assert
			subsriberMock1.Verify(x => x.OnInt(3), Times.Once);
			subsriberMock2.Verify(x => x.OnInt(3), Times.Once);
		}
	}


}
