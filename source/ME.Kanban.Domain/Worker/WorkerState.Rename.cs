using System.Collections.Generic;

namespace ME.Kanban.Domain.Worker
{
    public partial class WorkerState
    {

        public void Apply(Renamed @event)
        {
            this.GivenName = @event.GivenName;
            this.Surname = @event.Surname;
        }
    }
}
