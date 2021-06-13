using System;
using System.Collections.Generic;
using System.Linq;
using ME.RecordPlayer.EventSourcing;

namespace ME.Kanban.Domain.Project
{
    public partial class ProjectEntity
    {
        protected ProjectState State;

        public ProjectEntity(string id, Recorder recorder)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id), "A unique project entity id is required.");
            if (recorder is null) throw new ArgumentNullException(nameof(recorder), "Event sourced entities require a valid recorder to function.");

            this.ActorId = id;
            this.Recorder = recorder;
        }

        public string ActorId { get; private set; }
        public Recorder Recorder { get; private set; }
    }
}
