using ME.RecordPlayer.EventSourcing.Events;
using ME.RecordPlayer.EventSourcing.SnapShots;
using System;

namespace ME.RecordPlayer.EventSourcing.Tests.SampleEntity
{
  public class DocumentEntity
  {
    public string ActorId = string.Empty;
    public Guid ClientId = Guid.NewGuid();
    public Guid DocumentId = Guid.NewGuid();
    public Guid EmployeeId = Guid.NewGuid();
    public DateTime Now = DateTime.UtcNow;
    public IProvider Provider = null;
    public Recorder Recorder = null;
    public DocumentState State = null;
    public Guid UserId = Guid.NewGuid();
    public Uploaded Event1 => new Uploaded(this.DocumentId, "File.pdf", "Unit Test File", this.Now, this.Now, this.ClientId, this.UserId, this.Now);

    public EmployeeAssigned Event2 => new EmployeeAssigned(this.DocumentId, this.EmployeeId, "Mark", "ME", "Ewer", "42", "Number-1", this.ClientId, this.UserId, this.Now);

    public void ApplyEvent(object @event)
    {
      switch (@event)
      {
        case RecordedEvent recorded:
          State.ApplyEvent(recorded.Data);
          break;

        case ReplayEvent replay:
          State.ApplyEvent(replay.Data);
          break;

        case RecoverEvent recover:
          State.ApplyEvent(recover.Data);
          break;

        default:
          State.ApplyEvent(@event);
          break;
      }
    }

    public void ApplySnapshot(Snapshot snap)
    {
      if (snap.State is DocumentState)
        State = snap.State as DocumentState;
      else
        throw new InvalidCastException("This is not the right kind of state object");
    }

    public DocumentState GetState() => State;

    public DisplayNameChanged Rename(int renameCounter) => new DisplayNameChanged(this.DocumentId, $"Unit Test File Renamed {renameCounter}", this.ClientId, this.UserId, this.Now);
  }
}
