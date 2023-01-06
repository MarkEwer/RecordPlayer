using ME.RecordPlayer.EventSourcing.Events;
using ME.RecordPlayer.EventSourcing.Sqlite;
using ME.RecordPlayer.EventSourcing.Tests.SampleEntity;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ME.RecordPlayer.EventSourcing.Tests
{
  public class EventStorageTests
  {
    internal TestHelper SUT { get; set; }

    [Fact]
    public async Task Given_EventProvider_Should_Replay_Event_Records()
    {
      await Scenario
          .Given(async () => await A_New_DocumentState_Entity_Exists("db1"))
            .And(async () => await Three_Events_Had_Previously_Been_Recorded())
            .And(async () => await Our_Entity_Is_Configured_For_EventSourcing())

           .When(async () => await We_Restore_The_Entity_From_The_Saved_Previously_Recorded_Events())

           .Then(() => Assert.Equal(SUT.ClientId, SUT.State.ClientId))
            .And(() => Assert.Equal("Unit Test File Renamed", SUT.State.DisplayName))
            .And(() => Assert.Equal(SUT.DocumentId, SUT.State.DocumentId))
            .And(() => Assert.Equal(SUT.Now, SUT.State.Received))
            .And(() => Assert.Equal(SUT.Now, SUT.State.Uploaded))
            .And(() => Assert.Equal("Number-1", SUT.State.Owner.AssignmentId))
            .And(() => Assert.Equal(SUT.ClientId, SUT.State.Owner.ClientId))
            .And(() => Assert.Equal("42", SUT.State.Owner.CompanyId))
            .And(() => Assert.Equal(SUT.EmployeeId, SUT.State.Owner.EmployeeId))
            .And(() => Assert.Equal("Mark", SUT.State.Owner.GivenName))
            .And(() => Assert.Equal("ME", SUT.State.Owner.PreferredName))
            .And(() => Assert.Equal("Ewer", SUT.State.Owner.Surname));
    }

    [Fact]
    public async Task Given_EventProvider_Should_Store_Event_Records()
    {
      await Scenario
          .Given(async () => await A_New_DocumentState_Entity_Exists("db2"))
            .And(async () => await Our_Entity_Is_Configured_For_EventSourcing())

           .When(async () => await We_Record_Three_Events_Now())

           .Then(() => Assert.Equal(SUT.ClientId, SUT.State.ClientId))
            .And(() => Assert.Equal("Unit Test File Renamed", SUT.State.DisplayName))
            .And(() => Assert.Equal(SUT.DocumentId, SUT.State.DocumentId))
            .And(() => Assert.Equal(SUT.Now, SUT.State.Received))
            .And(() => Assert.Equal(SUT.Now, SUT.State.Uploaded))
            .And(() => Assert.Equal("Number-1", SUT.State.Owner.AssignmentId))
            .And(() => Assert.Equal(SUT.ClientId, SUT.State.Owner.ClientId))
            .And(() => Assert.Equal("42", SUT.State.Owner.CompanyId))
            .And(() => Assert.Equal(SUT.EmployeeId, SUT.State.Owner.EmployeeId))
            .And(() => Assert.Equal("Mark", SUT.State.Owner.GivenName))
            .And(() => Assert.Equal("ME", SUT.State.Owner.PreferredName))
            .And(() => Assert.Equal("Ewer", SUT.State.Owner.Surname));
    }

    private async Task A_New_DocumentState_Entity_Exists(string dbFileName)
    {
      var t = Task.Run(() =>
      {
        SUT = new TestHelper();
        SUT.ActorId = Guid.NewGuid().ToString();
        SUT.State = new DocumentState();
        if (File.Exists($"unit_test_{dbFileName}.db"))
          File.Delete($"unit_test_{dbFileName}.db");
        SUT.EventStore = new SqliteProvider(new SqliteConnectionStringBuilder($"Data Source=unit_test_{dbFileName}.db"));
      });
      await t.WaitAsync(TimeSpan.FromSeconds(3));
    }

    private async Task Our_Entity_Is_Configured_For_EventSourcing()
    {
      var t = Task.Run(() =>
      {
        SUT.Recorder = Recorder.WithEventSourcing(SUT.EventStore, SUT.ActorId, SUT.ApplyEvent);
      });
      await t.WaitAsync(TimeSpan.FromSeconds(3));
    }

    private async Task Three_Events_Had_Previously_Been_Recorded()
    {
      await SUT.EventStore.RecordEventAsync(SUT.ActorId, 1, SUT.Event1);
      await SUT.EventStore.RecordEventAsync(SUT.ActorId, 2, SUT.Event2);
      await SUT.EventStore.RecordEventAsync(SUT.ActorId, 3, SUT.Event3);
    }

    private async Task We_Record_Three_Events_Now()
    {
      await SUT.Recorder.RecordEventAsync(SUT.Event1);
      await SUT.Recorder.RecordEventAsync(SUT.Event2);
      await SUT.Recorder.RecordEventAsync(SUT.Event3);
    }

    private async Task We_Restore_The_Entity_From_The_Saved_Previously_Recorded_Events()
    {
      await SUT.Recorder.RecoverStateAsync();
    }

    internal class TestHelper
    {
      internal string ActorId = string.Empty;
      internal Guid ClientId = Guid.NewGuid();
      internal Guid DocumentId = Guid.NewGuid();
      internal Guid EmployeeId = Guid.NewGuid();
      internal IEventStore EventStore = null;
      internal DateTime Now = DateTime.UtcNow;
      internal Recorder Recorder = null;
      internal DocumentState State = null;
      internal Guid UserId = Guid.NewGuid();

      internal Uploaded Event1 => new Uploaded(this.DocumentId, "File.pdf", "Unit Test File", this.Now, this.Now, this.ClientId, this.UserId, this.Now);

      internal EmployeeAssigned Event2 => new EmployeeAssigned(this.DocumentId, this.EmployeeId, "Mark", "ME", "Ewer", "42", "Number-1", this.ClientId, this.UserId, this.Now);

      internal DisplayNameChanged Event3 => new DisplayNameChanged(this.DocumentId, "Unit Test File Renamed", this.ClientId, this.UserId, this.Now);

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
    }
  }
}
