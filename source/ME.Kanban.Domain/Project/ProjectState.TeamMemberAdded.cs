namespace ME.Kanban.Domain.Project
{
  public record TeamMemberAdded(string Name, string Role);

  public partial class ProjectState
  {
    public void Apply(TeamMemberAdded @event)
    {
      if (this._members.ContainsKey(@event.Name))
      {
        this._members[@event.Name] = @event.Role;
      }
      else
      {
        this._members.Add(@event.Name, @event.Role);
      }
    }
  }
}
