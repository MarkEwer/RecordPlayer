using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ME.Kanban.Domain.Project
{
  public partial class ProjectState
  {
    private readonly SortedDictionary<string, string> _members = new();

    #region Predictable State Tracking Properties
    public string Name { get; private set; }
    public string Description { get; private set; }
    public ProjectStatuses Status { get; private set; }
    public IImmutableDictionary<string, string> Members => _members.ToImmutableDictionary();

    #endregion Predictable State Tracking Properties

    #region Project Enumerations
    public enum ProjectStatuses : short
    {
      Unknown = 0,
      Active = 1,
      Supsended = 2,
      Closed = 3,
      Complete = 4
    }
    #endregion Project Enumerations
  }
}
