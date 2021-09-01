using ME.RecordPlayer.EventSourcing.SnapShots;

namespace ME.RecordPlayer.EventSourcing.LiteDb
{
  public record LiteDbSnapshot(string ActorName, string Json, long Index) : Snapshot(Json, Index)
  {
    public string Id => $"{ActorName}-snapshot-{Index}";
  }
}
