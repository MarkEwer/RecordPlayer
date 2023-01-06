using ME.RecordPlayer.EventSourcing.Events;
using ME.RecordPlayer.EventSourcing.InMemory;
using ME.RecordPlayer.EventSourcing.Snapshots.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ME.RecordPlayer.EventSourcing.Tests
{
  public class Snapshot_Strategy_Scenarios
  {
    public record IntegerAmountEvent(int Amount);

    [Fact]
    public async Task Given_An_IntervalStrategy_Should_Save_Snapshot_Accordingly()
    {
      int state = 0;
      InMemoryProvider provider = null;
      string actorId = string.Empty;
      Recorder intervalSnapshotRecorder = null;
      Dictionary<long, object> snapshots = null;

      await Scenario
          .Given(() => state = 1)
            .And(() => provider = new InMemoryProvider())
            .And(() => actorId = Guid.NewGuid().ToString())
            .And(() => intervalSnapshotRecorder = Recorder.WithEventSourcingAndSnapshotting(
                                     eventStore: provider,
                                     snapshotStore: provider,
                                     actorId,
                                     @event => { state *= ((IntegerAmountEvent)@event.Data).Amount; },
                                     snapshot => { state = (int)snapshot.State; },
                                     new IntervalStrategy(1), () => state))

           .When(async () => await intervalSnapshotRecorder.RecordEventAsync(new IntegerAmountEvent(2)))
            .And(async () => await intervalSnapshotRecorder.RecordEventAsync(new IntegerAmountEvent(2)))
            .And(async () => await intervalSnapshotRecorder.RecordEventAsync(new IntegerAmountEvent(2)))

           .Then(() => snapshots = provider.GetSnapshots(actorId))
            .And(() => Assert.Equal(3, snapshots.Count))
            .And(() => Assert.Equal(2, snapshots[0]))
            .And(() => Assert.Equal(4, snapshots[1]))
            .And(() => Assert.Equal(8, snapshots[2]));
    }

    [Fact]
    public async Task Validate_EventTypeStrategy_Should_Snapshot_According_To_The_Event_Type()
    {
      ISnapshotStrategy strategy = null;
      bool result = false;

      await Scenario
          .Given(() => strategy = new EventTypeStrategy(typeof(int)))

           .When(() => result = strategy.ShouldTakeSnapshot(new RecordedEvent(1, 0)))
           .Then(() => Assert.True(result))

           .When(() => result = strategy.ShouldTakeSnapshot(new RecordedEvent("not an int", 0)))
           .Then(() => Assert.False(result));
    }

    [Theory, InlineData(1, new[] { 1, 2, 3, 4, 5 }), InlineData(2, new[] { 2, 4, 6, 8, 10 }), InlineData(5, new[] { 5, 10, 15, 20, 25 })]
    public async Task Validate_IntervalStrategy_Should_Snapshot_According_To_The_Interval(int interval, int[] expected)
    {
      ISnapshotStrategy strategy = null;
      Dictionary<int, bool> result = new Dictionary<int, bool>();

      await Scenario
          .Given(() => strategy = new IntervalStrategy(interval))

           .When(() =>
           {
             for (var index = 1; index <= expected.Last(); index++)
               result.Add(index, strategy.ShouldTakeSnapshot(new RecordedEvent(null, index)));
           })

           .Then(() =>
           {
             foreach (var index in result.Keys)
               Assert.Equal(expected.Contains(index), result[index]);
           });
    }

    [Fact]
    public async Task Validate_TimeStrategy_Should_Snapshot_According_To_The_Timespan()
    {
      ISnapshotStrategy strategy = null;
      DateTime now = DateTime.MinValue;

      await Scenario
          .Given(() => now = DateTime.Parse("2000-01-01 12:00:00"))
            .And(() => strategy = new TimeStrategy(TimeSpan.FromSeconds(10), () => now))
           .Then(() => Assert.False(strategy.ShouldTakeSnapshot(new RecordedEvent(null, 0))))

           .When(() => now = now.AddSeconds(5))
           .Then(() => Assert.False(strategy.ShouldTakeSnapshot(new RecordedEvent(null, 0))))

           .When(() => now = now.AddSeconds(5))
           .Then(() => Assert.True(strategy.ShouldTakeSnapshot(new RecordedEvent(null, 0))))

           .When(() => now = now.AddSeconds(5))
           .Then(() => Assert.False(strategy.ShouldTakeSnapshot(new RecordedEvent(null, 0))))

           .When(() => now = now.AddSeconds(5))
           .Then(() => Assert.True(strategy.ShouldTakeSnapshot(new RecordedEvent(null, 0))));
    }
  }
}
