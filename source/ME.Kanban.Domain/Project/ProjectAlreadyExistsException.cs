using System;

namespace ME.Kanban.Domain.Project
{
    public class ProjectAlreadyExistsException : Exception
    {
        public ProjectAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
