using ME.RecordPlayer.EventSourcing.Events;

namespace ME.RecordPlayer.EventSourcing
{
  public interface ISnapshotStrategy
  {
    bool ShouldTakeSnapshot(RecordedEvent recordedEvent);
  }
}
