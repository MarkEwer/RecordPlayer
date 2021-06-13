using System.Collections.Generic;

namespace ME.Kanban.Domain.Worker
{
    public partial class WorkerState
    {
        public record Register(string Name);

        public void Apply(Register @event)
        {
            this.Name = @event.Name;
            this.Status = WorkerStatuses.Active;
        }
    }
}
