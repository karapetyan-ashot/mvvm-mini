using System;

namespace EasySoftware.MvvmMini.Core
{
	public interface IEventAggregator
	{
		void Subscribe<T>(Action<T> action, string key = null);
		void Unsubscribe<T>(Action<T> action, string key = null);
		void Publish<T>(T objectToSend, string key = null);
		void Reset();
	}
}