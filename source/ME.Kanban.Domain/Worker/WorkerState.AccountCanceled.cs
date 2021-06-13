using System.Collections.Generic;

namespace ME.Kanban.Domain.Worker
{

    public record AccountCanceled();

    public partial class WorkerState
    {

        public void Apply(AccountCanceled @event)
        {
            this.Status = WorkerStatuses.Canceled;
        }
    }
}
