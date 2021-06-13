namespace ME.RecordPlayer.EventSourcing.Events
{
    public record Event(object Data, long Index);
    public record RecordedEvent(object Data, long Index) : Event(Data, Index);
    public record ReplayEvent(object Data, long Index) : Event(Data, Index);
    public record RecoverEvent(object Data, long Index) : Event(Data, Index);
}
