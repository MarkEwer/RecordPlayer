using ME.RecordPlayer.EventSourcing.Events;
using ME.RecordPlayer.EventSourcing.SnapShots;
using System;

namespace DocumentLibrary.Core
{
  public class DocumentEntity 
  {
    private DocumentState _state = null;
    public DocumentState GetState() => _state;

    public void ApplyEvent(object @event)
    {
      switch (@event)
      {
        case RecordedEvent recorded:
          _state.ApplyEvent(recorded.Data);
          break;

        case ReplayEvent replay:
          _state.ApplyEvent(replay.Data);
          break;

        case RecoverEvent recover:
          _state.ApplyEvent(recover.Data);
          break;

        default:
          _state.ApplyEvent(@event);
          break;
      }
    }

    public void ApplySnapshot(Snapshot snap)
    {
      if (snap.State is DocumentState) _state = snap.State as DocumentState;
      else throw new InvalidCastException("This is not the right kind of state object");
    }
  }

  public class DocumentState
  {
    public Guid ClientId { get; private set; }

    public string DisplayName { get; private set; }

    public Guid DocumentId { get; private set; }

    public string Filename { get; private set; }

    public EmployeeState Owner { get; private set; }

    public DateTime Received { get; private set; }

    public DateTime Uploaded { get; private set; }

    public void ApplyEvent(object @event)
    {
      switch (@event)
      {
        case Uploaded evt:
          Apply(evt);
          break;

        case EmployeeAssigned evt:
          if (Owner is null) Owner = new EmployeeState();
          Owner.ApplyEvent(evt);
          break;

        case DisplayNameChanged evt:
          Apply(evt);
          break;
      }
    }

    private void Apply(Uploaded @event)
    {
      DocumentId = @event.DocumentId;
      Filename = @event.Filename;
      DisplayName = @event.DisplayName;
      Uploaded = @event.UploadedDate;
      Received = @event.ReceivedDate;
      ClientId = @event.ClientId;
    }

    private void Apply(DisplayNameChanged @event)
    {
      DisplayName = @event.DisplayName;
    }
  }

  public class EmployeeState
  {
    public Guid ClientId { get; private set; }

    public string CompanyId { get; private set; }

    public Guid EmployeeId { get; private set; }

    public string AssignmentId { get; private set; }

    public string GivenName { get; private set; }

    public string PreferredName { get; private set; }

    public string Surname { get; private set; }

    public void ApplyEvent(object @event)
    {
      switch (@event)
      {
        case EmployeeAssigned evt:
          Apply(evt);
          break;
      }
    }

    private void Apply(EmployeeAssigned @event)
    {
      EmployeeId = @event.EmployeeId;
      GivenName = @event.GivenName;
      PreferredName = @event.PreferredName;
      Surname = @event.Surname;
      CompanyId = @event.CompanyId;
      AssignmentId = @event.AssignmentId;
      ClientId = @event.ClientId;
    }
  }

  public record Uploaded(
    Guid DocumentId,
    string Filename,
    string DisplayName,
    DateTime UploadedDate,
    DateTime ReceivedDate,
    Guid ClientId,
    Guid User,
    DateTime EventDate);

  public record EmployeeAssigned(
      Guid DocumentId,
      Guid EmployeeId,
      string GivenName,
      string PreferredName,
      string Surname,
      string CompanyId,
      string AssignmentId,
      Guid ClientId,
      Guid User,
      DateTime EventDate);

  public record DisplayNameChanged(
      Guid DocumentId,
      string DisplayName,
      Guid ClientId,
      Guid User,
      DateTime EventDate);
}
