using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	/// <summary>
	/// Default thread-safe implementation of <see cref="IEventAggregator"/>.
	/// </summary>
	/// <remarks>
	/// Subscribers are held through weak references, so a subscription does not keep its target object alive.
	/// Use methods of a long-lived object as handlers. A lambda that captures local variables is owned only by the
	/// delegate and can be garbage collected, which silently ends the subscription.
	/// </remarks>
	public class EventAggregator : IEventAggregator
	{
		private readonly object _locker = new object();
		private readonly Dictionary<string, List<ActionWrapper>> _actions;

		#region constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="EventAggregator"/> class with no subscriptions.
		/// </summary>
		public EventAggregator()
		{
			lock (this._locker)
			{
				this._actions = new Dictionary<string, List<ActionWrapper>>();
			}
		}

		#endregion

		#region IEventAggregator
		/// <inheritdoc/>
		public void Subscribe<T>(Action<T> action, string key = null)
		{
			lock (this._locker)
			{
				ActionWrapper wrapper = new ActionWrapper(action);
				this.Add(key, wrapper);
			}
		}

		/// <inheritdoc/>
		public void Unsubscribe<T>(Action<T> action, string key = null)
		{
			lock (this._locker)
			{
				ActionWrapper wrapper = new ActionWrapper(action);
				this.Remove(key, wrapper);
			}
		}

		/// <inheritdoc/>
		public void Publish<T>(T objectToSend, string key = null)
		{
			lock (this._locker)
			{
				if (key == null)
					key = string.Empty;
				if (this._actions.ContainsKey(key))
				{
					Type paramType = typeof(T);
					List<ActionWrapper> foundList =
					this._actions[key].Where(x => x.ParameterType == paramType).ToList();

					foreach (ActionWrapper wrapper in foundList)
					{
						if (wrapper.IsAlive)
							wrapper.Publish(objectToSend);
						else
							this.Remove(key, wrapper);
					}
				}
			}
		}

		/// <inheritdoc/>
		public void Reset()
		{
			lock (this._locker)
			{
				this._actions.Clear();
			}
		}

		#endregion

		#region methods
		private void Add(string key, ActionWrapper wrapper)
		{
			if (key == null)
				key = string.Empty;
			ActionWrapper found = this.Find(key, wrapper);
			if (found == null)
			{
				if (!this._actions.ContainsKey(key))
					this._actions.Add(key, new List<ActionWrapper>());
				this._actions[key].Add(wrapper);
			}
		}

		private void Remove(string key, ActionWrapper wrapper)
		{
			if (key == null)
				key = string.Empty;
			ActionWrapper found = this.Find(key, wrapper);
			if (found != null)
			{
				this._actions[key].Remove(wrapper);
			}
		}

		private ActionWrapper Find(string key, ActionWrapper wrapper)
		{
			ActionWrapper found = null;
			if (this._actions.ContainsKey(key))
			{
				found = this._actions[key].SingleOrDefault(x => x.Equals(wrapper));
			}

			return found;
		}

		#endregion

		#region nested types
		private class ActionWrapper
		{
			private WeakReference _object { get; set; }
			private MethodInfo _method { get; set; }

			public ActionWrapper(object action)
			{
				if (action == null)
					throw new ArgumentNullException("action must be Action<T>");

				Type actionType = action.GetType();
				if (!actionType.IsGenericType)
					throw new ArgumentException("action must be Action<T>");

				this._method = actionType.GetProperty("Method").GetValue(action) as MethodInfo;
				if (!this._method.IsStatic)
					this._object = new WeakReference(actionType.GetProperty("Target").GetValue(action));

				this.ParameterType = actionType.GetGenericArguments()[0];

			}

			public Type ParameterType { get; private set; }

			public bool IsAlive
			{
				get
				{
					if (this._object != null)
						return this._object.IsAlive;
					return true;
				}
			}

			public void Publish(object objectToSend)
			{
				object target = null;
				if (this._object != null)
					target = this._object.Target;

				this._method.Invoke(target, new object[] { objectToSend });
			}

			public override bool Equals(object obj)
			{
				ActionWrapper wrapper = obj as ActionWrapper;
				if (wrapper == null)
					return false;
				if (this.ParameterType != wrapper.ParameterType)
					return false;
				if (this._object == null && wrapper._object == null)
					return true;
				else
					if (this._object != null && wrapper._object != null)
				{
					if (this._object.Target != wrapper._object.Target)
						return false;
				}
				else
					return false;

				if (this._method != wrapper._method)
					return false;
				return true;
			}
		}

		#endregion

	}
}