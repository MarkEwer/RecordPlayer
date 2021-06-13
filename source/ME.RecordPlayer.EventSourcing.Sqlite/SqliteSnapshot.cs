using ME.RecordPlayer.EventSourcing.SnapShots;

namespace ME.RecordPlayer.EventSourcing.Sqlite
{
    public record SqliteSnapshot(string ActorName, string Json, long Index) : Snapshot(Json, Index)
    {
        public string Id => $"{ActorName}-snapshot-{Index}";
    }
}
