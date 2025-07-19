using System;
using NUnit.Framework;

namespace TravisRFrench.Signals
{
    public class SignalBusTests
    {
        private class TestSignal
        {
            public int Value { get; set; }
        }

        /// <summary>
        /// Verifies that a subscribed listener receives the signal when it is published.
        /// </summary>
        [Test]
        public void GivenSubscribedListener_WhenSignalIsPublished_ThenListenerReceivesSignal()
        {
            /* GIVEN */
            var bus = new SignalBus();
            TestSignal? received = null;
            bus.Subscribe<TestSignal>(s => received = s);

            /* WHEN */
            var signal = new TestSignal { Value = 42 };
            bus.Publish(signal);

            /* THEN */
            Assert.That(received, Is.Not.Null);
            Assert.That(received!.Value, Is.EqualTo(42));
        }

        /// <summary>
        /// Ensures that a listener that has been unsubscribed does not receive the signal.
        /// </summary>
        [Test]
        public void GivenUnsubscribedListener_WhenSignalIsPublished_ThenListenerDoesNotReceiveSignal()
        {
            /* GIVEN */
            var bus = new SignalBus();
            TestSignal? received = null;
            Action<TestSignal> listener = s => received = s;
            bus.Subscribe(listener);
            bus.Unsubscribe(listener);

            /* WHEN */
            bus.Publish(new TestSignal { Value = 99 });

            /* THEN */
            Assert.That(received, Is.Null);
        }

        /// <summary>
        /// Checks that disposing a signal scope removes all its tracked subscriptions.
        /// </summary>
        [Test]
        public void GivenScopedSubscription_WhenScopeDisposed_ThenSubscriptionIsAutomaticallyRemoved()
        {
            /* GIVEN */
            var bus = new SignalBus();
            TestSignal? received = null;
            var scope = bus.CreateScope();
            scope.Subscribe<TestSignal>(s => received = s);
            scope.Dispose();

            /* WHEN */
            bus.Publish(new TestSignal { Value = 13 });

            /* THEN */
            Assert.That(received, Is.Null);
        }

        /// <summary>
        /// Tests that a listener subscribed via a scope receives a signal while the scope is active.
        /// </summary>
        [Test]
        public void GivenScopedSubscription_WhenSignalPublishedWithinScope_ThenListenerReceivesSignal()
        {
            /* GIVEN */
            var bus = new SignalBus();
            TestSignal? received = null;
            var scope = bus.CreateScope();
            scope.Subscribe<TestSignal>(s => received = s);

            /* WHEN */
            bus.Publish(new TestSignal { Value = 77 });

            /* THEN */
            Assert.That(received, Is.Not.Null);
            Assert.That(received!.Value, Is.EqualTo(77));
        }
    }
}
