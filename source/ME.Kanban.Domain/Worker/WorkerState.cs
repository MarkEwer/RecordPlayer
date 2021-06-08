using System.Collections.Generic;

namespace ME.Kanban.Domain.Worker
{
  public partial class WorkerState
  {
    #region Predictable State Tracking Properties
    public string Name { get; private set; }
    public string GivenName { get; private set; }
    public string Surname { get; private set; }
    public WorkerStatuses Status { get; private set; }
    public IDictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();

    #endregion Predictable State Tracking Properties

    #region Worker Enumerations
    public enum WorkerStatuses : short
    {
      Unknown = 0,
      Active = 1,
      Canceled = 2
    }
    #endregion Worker Enumerations
  }
}
