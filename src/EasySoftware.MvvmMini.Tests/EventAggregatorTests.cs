using EasySoftware.MvvmMini.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

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
        public void EventAggregator_Publish_SubscriberCalled()
        {
            // arrange
            var subsriber = Substitute.For<ISubscriber>();
            var eventAggregator = new EventAggregator();
            eventAggregator.Subscribe<int>(subsriber.OnInt);

            // act
            eventAggregator.Publish(3);

            // assert
            subsriber.Received(1).OnInt(3);
        }

        [TestMethod]
        public void EventAggregator_Publish_SubscribersCalled()
        {
            // arrange
            var subsriber1 = Substitute.For<ISubscriber>();
            var subsriber2 = Substitute.For<ISubscriber>();
            var eventAggregator = new EventAggregator();
            eventAggregator.Subscribe<int>(subsriber1.OnInt);
            eventAggregator.Subscribe<int>(subsriber2.OnInt);

            // act
            eventAggregator.Publish<int>(3);

            // assert
            subsriber1.Received(1).OnInt(3);
            subsriber2.Received(1).OnInt(3);
        }

        [TestMethod]
        public void EventAggregator_NamedPublish_SubscribersCalled()
        {
            // arrange
            string key = "key";
            var subsriber1WithKey = Substitute.For<ISubscriber>();
            var subsriber2WithKey = Substitute.For<ISubscriber>();
            var subsriber1WithoutKey = Substitute.For<ISubscriber>();
            var subsriber1WithoutWrongKey = Substitute.For<ISubscriber>();
            IEventAggregator eventAggregator = new EventAggregator();
            eventAggregator.Subscribe<int>(subsriber1WithKey.OnInt, key);
            eventAggregator.Subscribe<int>(subsriber2WithKey.OnInt, key);
            eventAggregator.Subscribe<int>(subsriber1WithoutKey.OnInt);
            eventAggregator.Subscribe<int>(subsriber1WithoutWrongKey.OnInt, "wrongKey");

            // act
            eventAggregator.Publish(3, key);

            // assert
            subsriber1WithKey.Received(1).OnInt(3);
            subsriber2WithKey.Received(1).OnInt(3);
            subsriber1WithoutKey.Received(0).OnInt(3);
            subsriber1WithoutWrongKey.Received(0).OnInt(3);
        }
    }
}
