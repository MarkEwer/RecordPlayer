using System;

namespace ME.Kanban.Domain.Project
{
    public record CreateProject(string Name, string Description);

    public partial class ProjectEntity
    {
        protected void Handle(CreateProject command)
        {
            if (string.IsNullOrEmpty(command.Name)) throw new ArgumentNullException(nameof(CreateProject.Name), "A project name is required.");
            if (string.IsNullOrEmpty(command.Description)) throw new ArgumentNullException(nameof(CreateProject.Description), "A project description is required.");
            if (this.State != null) throw new ProjectAlreadyExistsException("Projects can only be created once.");

            var @event = new ProjectStarted(command.Name, command.Description);

        }
    }
}
