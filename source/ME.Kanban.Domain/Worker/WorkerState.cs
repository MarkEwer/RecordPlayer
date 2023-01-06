using System.Collections.Generic;
using System.Collections.Immutable;

namespace ME.Kanban.Domain.Worker
{
  public partial class WorkerState
  {
    private readonly SortedDictionary<string, string> _roles = new();

    #region Predictable State Tracking Properties

    public string GivenName { get; private set; }
    public string Name { get; private set; }
    public IImmutableDictionary<string, string> Roles => _roles.ToImmutableDictionary();
    public WorkerStatuses Status { get; private set; }
    public string Surname { get; private set; }

    #endregion Predictable State Tracking Properties
  }
}
