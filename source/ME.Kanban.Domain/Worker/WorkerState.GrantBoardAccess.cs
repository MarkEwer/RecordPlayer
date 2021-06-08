using System.Collections.Generic;

namespace ME.Kanban.Domain.Worker
{
  public partial class WorkerState
  {
    public record GrantBoardAccess(string BoardId, string Role);

    public void Apply(GrantBoardAccess @event) 
    { 
      if(this.Roles.ContainsKey(@event.BoardId))
      {
        if(!string.IsNullOrEmpty(@event.Role))
        { 
          this.Roles[@event.BoardId] = @event.Role;
        }
        else
        {
          _ = this.Roles.Remove(@event.BoardId);
        }
      }
      else
      {
        if (!string.IsNullOrEmpty(@event.Role))
        {
          this.Roles.Add(@event.BoardId, @event.Role);
        }
      }
    }
  }
}
