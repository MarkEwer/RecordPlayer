using System;
using System.Runtime.Serialization;

namespace ME.Kanban.Domain.Project.Exceptions
{
  public class ProjectAlreadyExistsException : Exception
  {
    public ProjectAlreadyExistsException()
    {
    }

    public ProjectAlreadyExistsException(string message) : base(message)
    {
    }

    public ProjectAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ProjectAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
