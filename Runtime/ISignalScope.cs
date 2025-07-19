using System;

namespace TravisRFrench.Signals
{
	public interface ISignalScope : IDisposable
	{
		/// <summary>
		/// Subscribe to a signal of type T within this scope.
		/// When the scope is disposed, the subscription will be automatically removed.
		/// </summary>
		void Subscribe<T>(Action<T> callback);

		/// <summary>
		/// Unsubscribe from a signal of type T within this scope.
		/// Optional, as all tracked subscriptions are removed on Dispose().
		/// </summary>
		void Unsubscribe<T>(Action<T> callback);
	}
}
