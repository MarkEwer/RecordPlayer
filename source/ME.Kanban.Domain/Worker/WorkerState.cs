namespace ME.Kanban.Domain.Worker
{
  public class WorkerState
  {
    #region Predictable State Tracking Properties

    public string GivenName { get; private set; }

    public string Surname { get; private set; }

    public string Name { get; private set; }

    public string Role { get; private set; }

    #endregion Predictable State Tracking Properties

    #region Appling Events

    #endregion Appling Events
  }
}
