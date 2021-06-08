using System.Collections.Generic;

namespace ME.Kanban.Domain.Worker
{
  public partial class WorkerState
  {
    public record Rename(string GivenName, string Surname);

    public void Apply(Rename @event) 
    { 
      this.GivenName = @event.GivenName;
      this.Surname = @event.Surname;
    }
  }
}
