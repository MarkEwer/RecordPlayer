﻿using ME.RecordPlayer.EventSourcing.InMemory;
using ME.RecordPlayer.EventSourcing.Snapshots.Strategies;
using ME.RecordPlayer.EventSourcing.SnapShots;
using ME.RecordPlayer.EventSourcing.Sqlite;
using ME.RecordPlayer.EventSourcing.Tests.SampleEntity;
using Microsoft.Data.Sqlite;
using System;
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
          .Given(async () => await A_New_DocumentState_Entity_Exists())
            .And(async () => await Three_Events_Had_Previously_Been_Recorded())
            .And(async () => await Our_Entity_Is_Configured_For_EventSourcing())
           .When(async () => await We_Restore_The_Entity_From_The_Saved_Previously_Recorded_Events())

           .Then(() => Assert.Equal(SUT.ClientId, SUT.State.ClientId))
            .And(() => Assert.Equal("Unit Test File Renamed", SUT.State.DisplayName))
            .And(() => Assert.Equal(SUT.DocumentId, SUT.State.DocumentId))
            .And(() => Assert.Equal(SUT.Now, SUT.State.Recieved))
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
          .Given(async () => await A_New_DocumentState_Entity_Exists())
            .And(async () => await Our_Entity_Is_Configured_For_EventSourcing())
           .When(async () => await We_Record_Three_Events_Now())

           .Then(() => Assert.Equal(SUT.ClientId, SUT.State.ClientId))
            .And(() => Assert.Equal("Unit Test File Renamed", SUT.State.DisplayName))
            .And(() => Assert.Equal(SUT.DocumentId, SUT.State.DocumentId))
            .And(() => Assert.Equal(SUT.Now, SUT.State.Recieved))
            .And(() => Assert.Equal(SUT.Now, SUT.State.Uploaded))
            .And(() => Assert.Equal("Number-1", SUT.State.Owner.AssignmentId))
            .And(() => Assert.Equal(SUT.ClientId, SUT.State.Owner.ClientId))
            .And(() => Assert.Equal("42", SUT.State.Owner.CompanyId))
            .And(() => Assert.Equal(SUT.EmployeeId, SUT.State.Owner.EmployeeId))
            .And(() => Assert.Equal("Mark", SUT.State.Owner.GivenName))
            .And(() => Assert.Equal("ME", SUT.State.Owner.PreferredName))
            .And(() => Assert.Equal("Ewer", SUT.State.Owner.Surname));
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

    private async Task Our_Entity_Is_Configured_For_EventSourcing()
    {
      await Task.Run(() =>
      {
        SUT.EventStore = new SqliteProvider(new SqliteConnectionStringBuilder("Data Source=Sharable;Mode=Memory;Cache=Shared"));
        SUT.Recorder = Recorder.WithEventSourcing(SUT.EventStore, SUT.ActorId, SUT.State.ApplyEvent);
      });
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
    }
  }

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
        //SUT.Provider = new SqliteProvider(new SqliteConnectionStringBuilder("Data Source=Sharable;Mode=Memory;Cache=Shared"));
        SUT.Provider = new InMemoryProvider();
        SUT.Recorder = Recorder.WithEventSourcingAndSnapshotting(SUT.Provider, SUT.Provider, SUT.ActorId, SUT.State.ApplyEvent, SUT.ApplySnapshot, new IntervalStrategy(5), SUT.GetState);
      });
    }

    internal class TestHelper
    {
      internal string ActorId = string.Empty;
      internal Guid ClientId = Guid.NewGuid();
      internal Guid DocumentId = Guid.NewGuid();
      internal Guid EmployeeId = Guid.NewGuid();
      internal IProvider Provider = null;
      internal DateTime Now = DateTime.UtcNow;
      internal Recorder Recorder = null;
      internal DocumentState State = null;
      internal Guid UserId = Guid.NewGuid();

      internal Uploaded Event1 => new Uploaded(this.DocumentId, "File.pdf", "Unit Test File", this.Now, this.Now, this.ClientId, this.UserId, this.Now);

      internal EmployeeAssigned Event2 => new EmployeeAssigned(this.DocumentId, this.EmployeeId, "Mark", "ME", "Ewer", "42", "Number-1", this.ClientId, this.UserId, this.Now);

      internal DisplayNameChanged Rename(int renameCounter) => new DisplayNameChanged(this.DocumentId, $"Unit Test File Renamed {renameCounter}", this.ClientId, this.UserId, this.Now);

      internal DocumentState GetState() => State;

      internal void ApplySnapshot(Snapshot snap)
      {
        if (snap.State is DocumentState) State = snap.State as DocumentState;
        else throw new InvalidCastException("This is not the right kind of state object");
      }
    }
  }
}
