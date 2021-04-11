using System;
using System.Threading.Tasks;
using ME.RecordPlayer.EventSourcing.Events;
using ME.RecordPlayer.EventSourcing.Exceptions;
using ME.RecordPlayer.EventSourcing.SnapShots;

namespace ME.RecordPlayer.EventSourcing
{

    public class Recorder
    {
        private readonly string _actorId;
        private readonly Action<Event>? _applyEvent;
        private readonly Action<Snapshot>? _applySnapshot;
        private readonly IEventStore _eventStore;
        private readonly Func<object>? _getState;
        private readonly ISnapshotStore _snapshotStore;
        private readonly ISnapshotStrategy? _snapshotStrategy;

        private Recorder(
            IEventStore eventStore,
            ISnapshotStore snapshotStore,
            string actorId,
            Action<Event>? applyEvent = null,
            Action<Snapshot>? applySnapshot = null,
            ISnapshotStrategy? snapshotStrategy = null,
            Func<object>? getState = null
        )
        {
            _eventStore = eventStore;
            _snapshotStore = snapshotStore;
            _actorId = actorId;
            _applyEvent = applyEvent;
            _applySnapshot = applySnapshot;
            _getState = getState;
            _snapshotStrategy = snapshotStrategy ?? new MockSnapshots();
        }

        public long Index { get; private set; } = -1;
        private bool UsingEventSourcing => _applyEvent is not null;
        private bool UsingSnapshotting => _applySnapshot is not null;

        public static Recorder WithEventSourcing(IEventStore eventStore, string actorId, Action<Event> applyEvent)
        {
            if (eventStore is null) throw new ArgumentNullException(nameof(eventStore));

            if (applyEvent is null) throw new ArgumentNullException(nameof(applyEvent));

            return new Recorder(eventStore, new MockSnapshotStore(), actorId, applyEvent);
        }

        public static Recorder WithEventSourcingAndSnapshotting(
            IEventStore eventStore,
            ISnapshotStore snapshotStore,
            string actorId,
            Action<Event> applyEvent,
            Action<Snapshot> applySnapshot
        )
        {
            if (eventStore is null) throw new ArgumentNullException(nameof(eventStore));

            if (snapshotStore is null) throw new ArgumentNullException(nameof(snapshotStore));

            if (applyEvent is null) throw new ArgumentNullException(nameof(applyEvent));

            if (applySnapshot is null) throw new ArgumentNullException(nameof(applySnapshot));

            return new Recorder(eventStore, snapshotStore, actorId, applyEvent, applySnapshot);
        }

        public static Recorder WithEventSourcingAndSnapshotting(
            IEventStore eventStore,
            ISnapshotStore snapshotStore,
            string actorId,
            Action<Event> applyEvent,
            Action<Snapshot> applySnapshot,
            ISnapshotStrategy snapshotStrategy,
            Func<object> getState
        )
        {
            if (eventStore is null) throw new ArgumentNullException(nameof(eventStore));

            if (snapshotStore is null) throw new ArgumentNullException(nameof(snapshotStore));

            if (applyEvent is null) throw new ArgumentNullException(nameof(applyEvent));

            if (applySnapshot is null) throw new ArgumentNullException(nameof(applySnapshot));

            if (snapshotStrategy is null) throw new ArgumentNullException(nameof(snapshotStrategy));

            if (getState is null) throw new ArgumentNullException(nameof(getState));

            return new Recorder(eventStore, snapshotStore, actorId, applyEvent, applySnapshot, snapshotStrategy,
                getState
            );
        }

        public static Recorder WithSnapshotting(
            ISnapshotStore snapshotStore,
            string actorId,
            Action<Snapshot> applySnapshot
        )
        {
            if (snapshotStore is null) throw new ArgumentNullException(nameof(snapshotStore));

            if (applySnapshot is null) throw new ArgumentNullException(nameof(applySnapshot));

            return new Recorder(new MockEventStore(), snapshotStore, actorId, null, applySnapshot);
        }

        public Task DeleteEventsAsync(long inclusiveToIndex) =>
                    _eventStore.DeleteEventsAsync(_actorId, inclusiveToIndex);

        public Task DeleteSnapshotsAsync(long inclusiveToIndex) =>
                    _snapshotStore.DeleteSnapshotsAsync(_actorId, inclusiveToIndex);

        public async Task RecordEventAsync(object @event)
        {
            if (!UsingEventSourcing) throw new EventSourcingNotConfiguredException("Events cannot be recorded without using Event Sourcing.");

            var recordedEvent = new RecordedEvent(@event, Index + 1);

            await _eventStore.RecordEventAsync(_actorId, recordedEvent.Index, recordedEvent.Data);

            Index++;

            _applyEvent?.Invoke(recordedEvent);

            if (_snapshotStrategy?.ShouldTakeSnapshot(recordedEvent) == true && _getState is not null)
            {
                var recordedSnapshot = new RecordedSnapshot(_getState(), recordedEvent.Index);

                await _snapshotStore.RecordSnapshotAsync(_actorId, recordedSnapshot.Index, recordedSnapshot.State);
            }
        }

        public Task RecordSnapshotAsync(object snapshot)
        {
            if (!UsingSnapshotting) return Task.CompletedTask;

            var recordedSnapshot = new RecordedSnapshot(snapshot, Index);

            return _snapshotStore.RecordSnapshotAsync(_actorId, recordedSnapshot.Index, snapshot);
        }

        /// <summary>
        ///   Recovers the actor to the latest state by recoving the newest snapshot then
        ///   replaying all events after that snapshot.
        /// </summary>
        /// <returns> </returns>
        public async Task RecoverStateAsync()
        {
            var (snapshot, lastSnapshotIndex) = await _snapshotStore.GetSnapshotAsync(_actorId);

            if (snapshot is not null && UsingSnapshotting)
            {
                Index = lastSnapshotIndex;
#pragma warning disable CS8602 // Dereference of a possibly null reference. Warning is invalid because we just checked the `UsingSnapshotting` property.
                _applySnapshot(new RecoverSnapshot(snapshot, lastSnapshotIndex));
#pragma warning restore CS8602
            }

            var fromEventIndex = Index + 1;

            await _eventStore.GetEventsAsync(
                _actorId,
                fromEventIndex,
                long.MaxValue,
                @event =>
                {
                    Index++;
                    _applyEvent?.Invoke(new RecoverEvent(@event, Index));
                }
            );
        }

        /// <summary>
        ///   Allows the replaying of events to rebuild state from a range. For example,
        ///   if we want to replay until just before something happened (i.e. unexpected
        ///   behavior of the system, bug, crash etc..) then apply some messages and
        ///   observe what happens.
        /// </summary>
        public async Task ReplayEvents(long fromIndex, long toIndex)
        {
            if (!UsingEventSourcing) throw new EventSourcingNotConfiguredException("Events cannot be replayed without using Event Sourcing.");

            Index = fromIndex;

            await _eventStore.GetEventsAsync(
                _actorId,
                fromIndex,
                toIndex,
                @event =>
                {
                    _applyEvent?.Invoke(new ReplayEvent(@event, Index));
                    Index++;
                }
            );
        }

        private class MockEventStore : IEventStore
        {
            public Task DeleteEventsAsync(string actorName, long inclusiveToIndex) => Task.FromResult(0);

            public Task<long> GetEventsAsync(string actorName, long indexStart, long indexEnd, Action<object> callback) => Task.FromResult(-1L);

            public Task<long> RecordEventAsync(string actorName, long index, object @event) => Task.FromResult(0L);
        }

        private class MockSnapshots : ISnapshotStrategy
        {
            public bool ShouldTakeSnapshot(RecordedEvent recordedEvent) => false;
        }

        private class MockSnapshotStore : ISnapshotStore
        {
            public Task DeleteSnapshotsAsync(string actorName, long inclusiveToIndex) => Task.FromResult(0);

            public Task<(object? Snapshot, long Index)> GetSnapshotAsync(string actorName)
                            => Task.FromResult<(object? Snapshot, long Index)>((null, 0));

            public Task RecordSnapshotAsync(string actorName, long index, object snapshot) => Task.FromResult(0);
        }
    }
}
