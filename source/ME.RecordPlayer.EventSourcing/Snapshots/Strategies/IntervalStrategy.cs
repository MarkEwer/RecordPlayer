using ME.RecordPlayer.EventSourcing.Events;

namespace ME.RecordPlayer.EventSourcing.Snapshots.Strategies
{
  public class IntervalStrategy : ISnapshotStrategy
  {
    private readonly int _eventsPerSnapshot;

    public IntervalStrategy(int eventsPerSnapshot) => _eventsPerSnapshot = eventsPerSnapshot;

    public bool ShouldTakeSnapshot(RecordedEvent recordedEvent) => recordedEvent.Index % _eventsPerSnapshot == 0;
  }
}
