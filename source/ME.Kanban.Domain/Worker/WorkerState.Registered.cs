namespace ME.Kanban.Domain.Worker
{
  public record Registered(string Name);

  public partial class WorkerState
  {
    public void Apply(Registered @event)
    {
      this.Name = @event.Name;
      this.Status = WorkerStatuses.Active;
    }
  }
}
