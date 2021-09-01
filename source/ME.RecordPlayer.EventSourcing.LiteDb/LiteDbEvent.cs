using ME.RecordPlayer.EventSourcing.Events;

namespace ME.RecordPlayer.EventSourcing.LiteDb
{
  public record LiteDbEvent(string ActorName, string Json, long Index) : Event(Json, Index)
  {
    public string Id => $"{ActorName}-event-{Index}";
  }
}
