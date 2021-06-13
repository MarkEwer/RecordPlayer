namespace ME.Kanban.Domain.Project
{
    public record ProjectStarted(string Name, string Description);

    public partial class ProjectState
    {
        public void Apply(ProjectStarted @event)
        {
            this.Name = @event.Name;
            this.Description = @event.Description;
        }
    }
}
