using ME.RecordPlayer.EventSourcing.Events;
using System;

namespace ME.RecordPlayer.EventSourcing.Snapshots.Strategies
{
  public class EventTypeStrategy : ISnapshotStrategy
  {
    private readonly Type _eventType;

    public EventTypeStrategy(Type eventType) => _eventType = eventType;

    public bool ShouldTakeSnapshot(RecordedEvent recordedEvent) => recordedEvent.Data.GetType() == _eventType;
  }
}
