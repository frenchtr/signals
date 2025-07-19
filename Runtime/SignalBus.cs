using System;
using System.Collections.Generic;

namespace TravisRFrench.Signals
{
	public class SignalBus : ISignalBus
	{
		private readonly Dictionary<Type, List<Delegate>> listeners = new();

		public void Subscribe<T>(Action<T> callback)
		{
			var type = typeof(T);
			if (!this.listeners.ContainsKey(type))
			{
				this.listeners[type] = new List<Delegate>();
			}

			this.listeners[type].Add(callback);
		}

		public void Unsubscribe<T>(Action<T> callback)
		{
			var type = typeof(T);
			if (this.listeners.TryGetValue(type, out var list))
			{
				list.Remove(callback);
				if (list.Count == 0)
				{
					this.listeners.Remove(type);
				}
			}
		}

		public void Publish<T>(T signal)
		{
			var type = typeof(T);
			if (this.listeners.TryGetValue(type, out var list))
			{
				// Clone to allow mutation during dispatch
				var copy = list.ToArray();
				foreach (var callback in copy)
				{
					((Action<T>)callback)?.Invoke(signal);
				}
			}
		}

		public ISignalScope CreateScope()
		{
			return new SignalScope(this);
		}
	}
}
