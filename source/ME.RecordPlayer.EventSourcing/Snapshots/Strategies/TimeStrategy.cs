using System;
using ME.RecordPlayer.EventSourcing.Events;

namespace ME.RecordPlayer.EventSourcing.Snapshots.Strategies
{
    public class TimeStrategy : ISnapshotStrategy
    {
        private readonly Func<DateTime> _getNow;
        private readonly TimeSpan _interval;
        private DateTime _lastTaken;

        public TimeStrategy(TimeSpan interval, Func<DateTime>? getNow = null)
        {
            _interval = interval;
            _getNow = getNow ?? (() => DateTime.Now);
            _lastTaken = _getNow();
        }

        public bool ShouldTakeSnapshot(RecordedEvent recordedEvent)
        {
            var now = _getNow();
            if (_lastTaken.Add(_interval) > now) return false;

            _lastTaken = now;
            return true;
        }
    }
}
