using System;
using System.Collections.Generic;

namespace TravisRFrench.Signals
{
	public class SignalScope : ISignalScope
	{
		private readonly ISignalBus bus;
		private readonly Dictionary<Type, List<Delegate>> tracked = new();

		public SignalScope(ISignalBus bus)
		{
			this.bus = bus;
		}

		public void Subscribe<T>(Action<T> callback)
		{
			var type = typeof(T);
			if (!this.tracked.ContainsKey(type))
			{
				this.tracked[type] = new List<Delegate>();
			}

			this.tracked[type].Add(callback);
			this.bus.Subscribe(callback);
		}

		public void Unsubscribe<T>(Action<T> callback)
		{
			var type = typeof(T);
			if (this.tracked.TryGetValue(type, out var list))
			{
				list.Remove(callback);
				if (list.Count == 0)
				{
					this.tracked.Remove(type);
				}
			}

			this.bus.Unsubscribe(callback);
		}

		public void Dispose()
		{
			foreach (var (type, delegates) in this.tracked)
			{
				var unsubscribeMethod = typeof(ISignalBus)
					.GetMethod(nameof(ISignalBus.Unsubscribe))
					?.MakeGenericMethod(type);

				if (unsubscribeMethod == null)
					continue;

				foreach (var callback in delegates)
				{
					unsubscribeMethod.Invoke(this.bus, new object[] { callback });
				}
			}

			this.tracked.Clear();
		}
	}
}
