using System;

namespace EasySoftware.MvvmMini.Core
{
	/// <summary>
	/// Publishes messages to subscribers without the publisher and the subscribers referencing each other.
	/// </summary>
	/// <remarks>
	/// A message reaches the subscribers registered for exactly its type <c>T</c> and the same key.
	/// </remarks>
	public interface IEventAggregator
	{
		/// <summary>
		/// Subscribes an action to messages of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The message type.</typeparam>
		/// <param name="action">The action to invoke when a message is published.</param>
		/// <param name="key">An optional key that narrows the subscription to a channel; <see langword="null"/> means the default channel.</param>
		void Subscribe<T>(Action<T> action, string key = null);

		/// <summary>
		/// Removes a subscription that was added with <see cref="Subscribe{T}(Action{T}, string)"/>.
		/// </summary>
		/// <typeparam name="T">The message type.</typeparam>
		/// <param name="action">The action to remove.</param>
		/// <param name="key">The key the action was subscribed with.</param>
		void Unsubscribe<T>(Action<T> action, string key = null);

		/// <summary>
		/// Publishes a message to all subscribers of type <typeparamref name="T"/> and the given key.
		/// </summary>
		/// <typeparam name="T">The message type.</typeparam>
		/// <param name="objectToSend">The message to send.</param>
		/// <param name="key">An optional key that selects the channel; <see langword="null"/> means the default channel.</param>
		void Publish<T>(T objectToSend, string key = null);

		/// <summary>
		/// Removes all subscriptions.
		/// </summary>
		void Reset();
	}
}