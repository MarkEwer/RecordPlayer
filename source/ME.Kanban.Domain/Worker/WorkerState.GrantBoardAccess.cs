using System.Collections.Generic;

namespace ME.Kanban.Domain.Worker
{
    public partial class WorkerState
    {
        public record GrantBoardAccess(string BoardId, string Role);

        public void Apply(GrantBoardAccess @event)
        {
            if (this.Roles.ContainsKey(@event.BoardId))
            {
                if (!string.IsNullOrEmpty(@event.Role))
                {
                    this._roles[@event.BoardId] = @event.Role;
                }
                else
                {
                    _ = this._roles.Remove(@event.BoardId);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(@event.Role))
                {
                    this._roles.Add(@event.BoardId, @event.Role);
                }
            }
        }
    }
}
