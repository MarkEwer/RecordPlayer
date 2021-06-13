using ME.RecordPlayer.EventSourcing.Events;

namespace ME.RecordPlayer.EventSourcing.Sqlite
{
    public record SqliteEvent(string ActorName, string Json, long Index) : Event(Json, Index)
    {
        public string Id => $"{ActorName}-event-{Index}";
    }
}
