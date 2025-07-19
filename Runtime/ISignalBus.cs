using System;

namespace TravisRFrench.Signals
{
	public interface ISignalBus
	{
		/// <summary>
		/// Subscribe to a global signal of type T.
		/// Use only for one-off or global handlers.
		/// </summary>
		void Subscribe<T>(Action<T> callback);

		/// <summary>
		/// Unsubscribe from a global signal of type T.
		/// </summary>
		void Unsubscribe<T>(Action<T> callback);

		/// <summary>
		/// Publish a signal to all listeners of type T.
		/// </summary>
		void Publish<T>(T signal);

		/// <summary>
		/// Create a manual (disposable) signal scope for tracking subscriptions.
		/// </summary>
		ISignalScope CreateScope();
	}
}
