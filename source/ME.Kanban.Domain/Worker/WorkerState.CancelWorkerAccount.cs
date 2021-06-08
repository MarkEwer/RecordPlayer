using System.Collections.Generic;

namespace ME.Kanban.Domain.Worker
{
  public partial class WorkerState
  {
    public record CancelWorkerAccount();

    public void Apply(CancelWorkerAccount @event)
    {
      this.Status = WorkerStatuses.Canceled;
    }
  }
}
