using System;
using System.Collections.Generic;
using System.Linq;

namespace ME.Kanban.Domain.Project
{
    public record StartProject(string Name, string Description);

    public record TeamMemberAdded(string Name, string Role);

}
