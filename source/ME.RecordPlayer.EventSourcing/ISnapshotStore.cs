using System.Threading.Tasks;

namespace ME.RecordPlayer.EventSourcing
{
    public interface ISnapshotStore
    {
        Task DeleteSnapshotsAsync(string actorName, long inclusiveToIndex);

        Task<(object? Snapshot, long Index)> GetSnapshotAsync(string actorName);

        Task RecordSnapshotAsync(string actorName, long index, object snapshot);
    }
}
