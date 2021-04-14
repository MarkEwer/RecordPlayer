using ME.RecordPlayer.EventSourcing.Events;
using ME.RecordPlayer.EventSourcing.Snapshots.Strategies;
using ME.RecordPlayer.EventSourcing.SnapShots;
using ME.RecordPlayer.EventSourcing.Sqlite;
using ME.RecordPlayer.EventSourcing.Tests.SampleEntity;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ME.RecordPlayer.EventSourcing.Tests
{
  public class EventAndSnapshotStorageTests
  {
    internal TestHelper SUT { get; set; }

    [Fact]
    public async Task Given_Event_And_Snapshot_Should_Replay_Snapshot_Then_Events()
    {
      await Scenario
          .Given(async () => await A_New_DocumentState_Entity_Exists())
            .And(async () => await Our_Entity_Is_Configured_For_EventSourcing_And_Snapshotting())

           .When(async () => await We_Record_Two_Events_Now())
            .And(async () => await We_Renam_The_Document_15_Times())

           .Then(() => Assert.Equal(SUT.ClientId, SUT.State.ClientId));
    }

    private async Task We_Record_Two_Events_Now()
    {
      await SUT.Recorder.RecordEventAsync(SUT.Event1);
      await SUT.Recorder.RecordEventAsync(SUT.Event2);
    }

    private async Task We_Renam_The_Document_15_Times()
    {
      for (int i = 1; i < 16; i++) await SUT.Recorder.RecordEventAsync(SUT.Rename(i));
    }

    private async Task A_New_DocumentState_Entity_Exists()
    {
      await Task.Run(() =>
      {
        SUT = new TestHelper();
        SUT.ActorId = Guid.NewGuid().ToString();
        SUT.State = new DocumentState();
      });
    }

    private async Task Our_Entity_Is_Configured_For_EventSourcing_And_Snapshotting()
    {
      await Task.Run(() =>
      {
        //SUT.Provider = new InMemoryProvider();

        if (File.Exists("unit_test.db")) File.Delete("unit_test.db");
        SUT.Provider = new SqliteProvider(new SqliteConnectionStringBuilder("Data Source=unit_test.db"));
        SUT.Recorder = Recorder.WithEventSourcingAndSnapshotting(SUT.Provider, SUT.Provider, SUT.ActorId, SUT.ApplyEvent, SUT.ApplySnapshot, new IntervalStrategy(5), SUT.GetState);
      });
    }
  }

  internal class TestHelper
  {
    internal string ActorId = string.Empty;
    internal Guid ClientId = Guid.NewGuid();
    internal Guid DocumentId = Guid.NewGuid();
    internal Guid EmployeeId = Guid.NewGuid();
    internal Guid UserId = Guid.NewGuid();
    internal IProvider Provider = null;
    internal DateTime Now = DateTime.UtcNow;
    internal Recorder Recorder = null;
    internal DocumentState State = null;

    internal Uploaded Event1 => new Uploaded(this.DocumentId, "File.pdf", "Unit Test File", this.Now, this.Now, this.ClientId, this.UserId, this.Now);

    internal EmployeeAssigned Event2 => new EmployeeAssigned(this.DocumentId, this.EmployeeId, "Mark", "ME", "Ewer", "42", "Number-1", this.ClientId, this.UserId, this.Now);

    internal DisplayNameChanged Rename(int renameCounter) => new DisplayNameChanged(this.DocumentId, $"Unit Test File Renamed {renameCounter}", this.ClientId, this.UserId, this.Now);

    internal DocumentState GetState() => State;

    internal void ApplyEvent(object @event)
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

    internal void ApplySnapshot(Snapshot snap)
    {
      if (snap.State is DocumentState) State = snap.State as DocumentState;
      else throw new InvalidCastException("This is not the right kind of state object");
    }
  }

  public class RestoringEntitiesFromSavedStateTests
  {
    internal TestHelper SUT { get; set; }

    public RestoringEntitiesFromSavedStateTests()
    {
      this.SUT = new TestHelper();
      this.SUT.ActorId = Guid.NewGuid().ToString("N");

      if (File.Exists($"unit_test_{nameof(RestoringEntitiesFromSavedStateTests)}.db"))
        File.Delete("unit_test_{nameof(RestoringEntitiesFromSavedStateTests)}.db");
      SUT.Provider = new SqliteProvider(new SqliteConnectionStringBuilder($"Data Source=unit_test_{nameof(RestoringEntitiesFromSavedStateTests)}.db"));
      SUT.Recorder = Recorder.WithEventSourcingAndSnapshotting(SUT.Provider, SUT.Provider, SUT.ActorId, SUT.ApplyEvent, SUT.ApplySnapshot, new IntervalStrategy(5), SUT.GetState);
    }

    [Fact]
    public async Task Entity_Can_Be_Recovered_From_Previous_State_Test()
    {
      await Given_An_Entity_Was_Saved_With_Multiple_Snapshots_And_Multiple_Events();
      await When_We_Close_The_Entity();
      await When_We_Attempt_To_Recover_The_Saved_Entity();
      await Then_It_Should_Have_These_State_Properties();
    }

    private async Task Given_An_Entity_Was_Saved_With_Multiple_Snapshots_And_Multiple_Events()
    {
      SUT.State = new DocumentState();
      for (int i = 1; i < 96; i++) await SUT.Recorder.RecordEventAsync(SUT.Rename(i));
    }

    private DocumentState oldState = null;

    private async Task When_We_Close_The_Entity()
    {
      oldState = SUT.State;
      SUT.State = null;
    }

    private async Task When_We_Attempt_To_Recover_The_Saved_Entity()
    {
      SUT.State = new DocumentState();
      await SUT.Recorder.RecoverStateAsync();
    }

    private async Task Then_It_Should_Have_These_State_Properties()
    {
      Assert.Equal(oldState.ClientId, SUT.State.ClientId);
      Assert.Equal(oldState.DisplayName, SUT.State.DisplayName);
      Assert.Equal(oldState.DocumentId, SUT.State.DocumentId);
      Assert.Equal(oldState.Filename, SUT.State.Filename);
      Assert.Equal(oldState.Recieved, SUT.State.Recieved);
      Assert.Equal(oldState.Uploaded, SUT.State.Uploaded);
    }
  }
}
