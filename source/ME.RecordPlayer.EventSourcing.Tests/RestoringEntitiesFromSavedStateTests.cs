using ME.RecordPlayer.EventSourcing.Snapshots.Strategies;
using ME.RecordPlayer.EventSourcing.Sqlite;
using ME.RecordPlayer.EventSourcing.Tests.SampleEntity;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ME.RecordPlayer.EventSourcing.Tests
{
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
      await Task.Run(() =>
      {
        oldState = SUT.State;
        SUT.State = null;
      });
    }

    private async Task When_We_Attempt_To_Recover_The_Saved_Entity()
    {
      SUT.State = new DocumentState();
      await SUT.Recorder.RecoverStateAsync();
    }

    private async Task Then_It_Should_Have_These_State_Properties()
    {
      await Task.Run(() =>
      {
        Assert.Equal(oldState.ClientId, SUT.State.ClientId);
        Assert.Equal(oldState.DisplayName, SUT.State.DisplayName);
        Assert.Equal(oldState.DocumentId, SUT.State.DocumentId);
        Assert.Equal(oldState.Filename, SUT.State.Filename);
        Assert.Equal(oldState.Received, SUT.State.Received);
        Assert.Equal(oldState.Uploaded, SUT.State.Uploaded);
      });
    }
  }
}
