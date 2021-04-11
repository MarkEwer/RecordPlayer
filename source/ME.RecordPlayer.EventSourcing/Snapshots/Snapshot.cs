namespace ME.RecordPlayer.EventSourcing.SnapShots
{
    public record Snapshot(object State, long Index);
    public record RecoverSnapshot(object State, long Index) : Snapshot(State, Index);
    public record RecordedSnapshot(object State, long Index) : Snapshot(State, Index);
}
