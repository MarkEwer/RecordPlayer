using System;
using ME.RecordPlayer.EventSourcing.Sqlite;
using ME.RecordPlayer.EventSourcing.Tests.SampleEntity;
using Microsoft.Data.Sqlite;
using Xunit;

namespace ME.RecordPlayer.EventSourcing.Tests
{
    public class EventStorageTests
    {
        [Fact]
        public void Given_EventProvider_Should_Replay_Event_Records()
        {
            Guid documentId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            Guid employeeId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            IEventStore eventStore = null;
            Recorder recorder = null;
            string actorId = string.Empty;
            DocumentState state = null;

            Scenario
                .Given(() => eventStore = new SqliteProvider(new SqliteConnectionStringBuilder("Data Source=Sharable;Mode=Memory;Cache=Shared")))
                  .And(() => actorId = Guid.NewGuid().ToString())
                  .And(() => state = new DocumentState())
                  .And(async () => await eventStore.RecordEventAsync(actorId, 1, new Uploaded(documentId, "File.pdf", "Unit Test File", now, now, clientId, userId, now)))
                  .And(async () => await eventStore.RecordEventAsync(actorId, 2, new EmployeeAssigned(documentId, employeeId, "Mark", "ME", "Ewer", "42", "Number-1", clientId, userId, now)))
                  .And(async () => await eventStore.RecordEventAsync(actorId, 3, new DisplayNameChanged(documentId, "Unit Test File Renamed", clientId, userId, now)))

                 .When(() => recorder = Recorder.WithEventSourcing(eventStore, actorId, state.ApplyEvent))
                  .And(async () => await recorder.RecoverStateAsync())

                 .Then(() => Assert.Equal(clientId, state.ClientId))
                  .And(() => Assert.Equal("Unit Test File Renamed", state.DisplayName))
                  .And(() => Assert.Equal(documentId, state.DocumentId))
                  .And(() => Assert.Equal(now, state.Recieved))
                  .And(() => Assert.Equal(now, state.Uploaded))
                  .And(() => Assert.Equal("Number-1", state.Owner.AssignmentId))
                  .And(() => Assert.Equal(clientId, state.Owner.ClientId))
                  .And(() => Assert.Equal("42", state.Owner.CompanyId))
                  .And(() => Assert.Equal(employeeId, state.Owner.EmployeeId))
                  .And(() => Assert.Equal("Mark", state.Owner.GivenName))
                  .And(() => Assert.Equal("ME", state.Owner.PreferredName))
                  .And(() => Assert.Equal("Ewer", state.Owner.Surname));
        }

        [Fact]
        public void Given_EventProvider_Should_Store_Event_Records()
        {
            Guid documentId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            Guid clientId = Guid.NewGuid();
            Guid employeeId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            IEventStore eventStore = null;
            Recorder recorder = null;
            string actorId = string.Empty;
            DocumentState state = null;

            Scenario
                .Given(() => eventStore = new SqliteProvider(new SqliteConnectionStringBuilder("Data Source=Sharable;Mode=Memory;Cache=Shared")))
                  .And(() => actorId = Guid.NewGuid().ToString())
                  .And(() => state = new DocumentState())
                  .And(() => recorder = Recorder.WithEventSourcing(eventStore, actorId, state.ApplyEvent))

                 .When(async () => await recorder.RecordEventAsync(new Uploaded(documentId, "File.pdf", "Unit Test File", now, now, clientId, userId, now)))
                  .And(async () => await recorder.RecordEventAsync(new EmployeeAssigned(documentId, employeeId, "Mark", "ME", "Ewer", "42", "Number-1", clientId, userId, now)))
                  .And(async () => await recorder.RecordEventAsync(new DisplayNameChanged(documentId, "Unit Test File Renamed", clientId, userId, now)))

                 .Then(() => Assert.Equal(clientId, state.ClientId))
                  .And(() => Assert.Equal("Unit Test File Renamed", state.DisplayName))
                  .And(() => Assert.Equal(documentId, state.DocumentId))
                  .And(() => Assert.Equal(now, state.Recieved))
                  .And(() => Assert.Equal(now, state.Uploaded))
                  .And(() => Assert.Equal("Number-1", state.Owner.AssignmentId))
                  .And(() => Assert.Equal(clientId, state.Owner.ClientId))
                  .And(() => Assert.Equal("42", state.Owner.CompanyId))
                  .And(() => Assert.Equal(employeeId, state.Owner.EmployeeId))
                  .And(() => Assert.Equal("Mark", state.Owner.GivenName))
                  .And(() => Assert.Equal("ME", state.Owner.PreferredName))
                  .And(() => Assert.Equal("Ewer", state.Owner.Surname));
        }
    }
}
