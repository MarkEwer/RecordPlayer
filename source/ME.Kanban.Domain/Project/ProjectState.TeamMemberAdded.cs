using System;
using System.Collections.Generic;
using System.Linq;

namespace ME.Kanban.Domain.Project
{
    public partial class ProjectState
    {
        public record TeamMemberAdded(string Name, string Role);

        public void Apply(TeamMemberAdded @event)
        {
            if (this._members.ContainsKey(@event.Name))
            {
                this._members[@event.Name] = @event.Role;
            }
            else
            {
                this._members.Add(@event.Name, @event.Role);
            }
        }
    }
}
