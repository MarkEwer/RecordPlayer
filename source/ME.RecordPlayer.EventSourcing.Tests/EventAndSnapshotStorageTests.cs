﻿using System;
using System.IO;
using System.Threading.Tasks;
using ME.RecordPlayer.EventSourcing.Snapshots.Strategies;
using ME.RecordPlayer.EventSourcing.Sqlite;
using ME.RecordPlayer.EventSourcing.Tests.SampleEntity;
using Microsoft.Data.Sqlite;
using Xunit;

namespace ME.RecordPlayer.EventSourcing.Tests
{
    public class EventAndSnapshotStorageTests
    {
        internal DocumentEntity SUT { get; set; }

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

        private async Task A_New_DocumentState_Entity_Exists()
        {
            await Task.Run(() =>
            {
                SUT = new DocumentEntity();
                SUT.ActorId = Guid.NewGuid().ToString();
                SUT.State = new DocumentState();
            });
        }

        private async Task Our_Entity_Is_Configured_For_EventSourcing_And_Snapshotting()
        {
            await Task.Run(() =>
            {
                if (File.Exists("unit_test.db")) File.Delete("unit_test.db");
                SUT.Provider = new SqliteProvider(new SqliteConnectionStringBuilder("Data Source=unit_test.db"));
                SUT.Recorder = Recorder.WithEventSourcingAndSnapshotting(SUT.Provider, SUT.Provider, SUT.ActorId, SUT.ApplyEvent, SUT.ApplySnapshot, new IntervalStrategy(5), SUT.GetState);
            });
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
    }
}
