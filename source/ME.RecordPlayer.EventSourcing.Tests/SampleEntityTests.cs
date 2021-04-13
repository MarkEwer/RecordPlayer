using ME.RecordPlayer.EventSourcing.Tests.SampleEntity;
using System;
using Xunit;

namespace ME.RecordPlayer.EventSourcing.Tests
{
  public class SampleEntityTests
  {
    [Fact]
    public void Given_A_DocumentState_Should_Apply_Events()
    {
      Guid documentId = Guid.NewGuid();
      Guid userId = Guid.NewGuid();
      Guid clientId = Guid.NewGuid();
      Guid employeeId = Guid.NewGuid();
      DateTime now = DateTime.UtcNow;
      DocumentState state = null;

      Scenario
          .Given(() => state = new DocumentState())
           .When(() => state.ApplyEvent(new Uploaded(documentId, "File.pdf", "Unit Test File", now, now, clientId, userId, now)))
            .And(() => state.ApplyEvent(new EmployeeAssigned(documentId, employeeId, "Mark", "ME", "Ewer", "42", "Number-1", clientId, userId, now)))
            .And(() => state.ApplyEvent(new DisplayNameChanged(documentId, "Unit Test File Renamed", clientId, userId, now)))
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
