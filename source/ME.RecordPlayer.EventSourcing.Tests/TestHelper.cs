//using ME.RecordPlayer.EventSourcing.Events;
//using ME.RecordPlayer.EventSourcing.SnapShots;
//using ME.RecordPlayer.EventSourcing.Tests.SampleEntity;
//using System;

//namespace ME.RecordPlayer.EventSourcing.Tests
//{
//  internal class TestHelper
//  {
//    internal string ActorId = string.Empty;
//    internal Guid ClientId = Guid.NewGuid();
//    internal Guid DocumentId = Guid.NewGuid();
//    internal Guid EmployeeId = Guid.NewGuid();
//    internal Guid UserId = Guid.NewGuid();
//    internal IProvider Provider = null;
//    internal DateTime Now = DateTime.UtcNow;
//    internal Recorder Recorder = null;
//    internal DocumentState State = null;

// internal Uploaded Event1 => new Uploaded(this.DocumentId, "File.pdf", "Unit Test File",
// this.Now, this.Now, this.ClientId, this.UserId, this.Now);

// internal EmployeeAssigned Event2 => new EmployeeAssigned(this.DocumentId,
// this.EmployeeId, "Mark", "ME", "Ewer", "42", "Number-1", this.ClientId, this.UserId,
// this.Now);

// internal DisplayNameChanged Rename(int renameCounter) => new
// DisplayNameChanged(this.DocumentId, $"Unit Test File Renamed {renameCounter}",
// this.ClientId, this.UserId, this.Now);

// internal DocumentState GetState() => State;

// internal void ApplyEvent(object @event) { switch (@event) { case RecordedEvent
// recorded: State.ApplyEvent(recorded.Data); break;

// case ReplayEvent replay: State.ApplyEvent(replay.Data); break;

// case RecoverEvent recover: State.ApplyEvent(recover.Data); break;

// default: State.ApplyEvent(@event); break; } }

//    internal void ApplySnapshot(Snapshot snap)
//    {
//      if (snap.State is DocumentState) State = snap.State as DocumentState;
//      else throw new InvalidCastException("This is not the right kind of state object");
//    }
//  }
//}
