using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ME.Kanban.Domain.Project
{
  public partial class ProjectState
  {
    public record StartProject(string Name, string Description);

    public void Apply(StartProject @event)
    {
      this.Name = @event.Name;
      this.Description = @event.Description;
    }
  }
}
