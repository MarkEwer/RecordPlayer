using System.Collections.Generic;

namespace ME.Kanban.Domain.Worker
{

    public partial class WorkerState
    {

        public void Apply(Registered @event)
        {
            this.Name = @event.Name;
            this.Status = WorkerStatuses.Active;
        }
    }
}
