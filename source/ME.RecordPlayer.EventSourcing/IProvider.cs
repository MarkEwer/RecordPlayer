namespace ME.RecordPlayer.EventSourcing
{
    public interface IProvider : IEventStore, ISnapshotStore
    {
    }
}
