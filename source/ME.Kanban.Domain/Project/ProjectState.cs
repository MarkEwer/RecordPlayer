using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ME.Kanban.Domain.Project
{
    public partial class ProjectState
    {
        private readonly SortedSet<string> _boards = new();
        private readonly SortedDictionary<string, string> _members = new();

        #region Predictable State Tracking Properties

        public IImmutableList<string> Boards => _boards.ToImmutableList();
        public string Description { get; private set; }
        public IImmutableDictionary<string, string> Members => _members.ToImmutableDictionary();
        public string Name { get; private set; }
        public ProjectStatuses Status { get; private set; }

        #endregion Predictable State Tracking Properties

        #region Project Event Apply Methods

        #endregion Project Event Apply Methods
    }
}
