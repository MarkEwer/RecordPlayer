namespace ME.Kanban.Domain.Worker
{
  public record Renamed(string GivenName, string Surname);

  public partial class WorkerState
  {
    public void Apply(Renamed @event)
    {
      this.GivenName = @event.GivenName;
      this.Surname = @event.Surname;
    }
  }
}
