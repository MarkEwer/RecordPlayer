﻿using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ME.RecordPlayer.EventSourcing.Sqlite
{
  public class SqliteProvider : IProvider
  {
#pragma warning disable SCS0028 // TypeNameHandling is set to other value than 'None' that may lead to deserialization vulnerability
    private static readonly JsonSerializerSettings AllTypeSettings = new() { TypeNameHandling = TypeNameHandling.All };
    private static readonly JsonSerializerSettings AutoTypeSettings = new() { TypeNameHandling = TypeNameHandling.Auto };
#pragma warning restore SCS0028 // TypeNameHandling is set to other value than 'None' that may lead to deserialization vulnerability
    private readonly SqliteConnectionStringBuilder _connectionStringBuilder;

    public SqliteProvider(SqliteConnectionStringBuilder connectionStringBuilder)
    {
      _connectionStringBuilder = connectionStringBuilder;

      using var connection = new SqliteConnection(ConnectionString);

      connection.Open();

      using var initEventsCommand = connection.CreateCommand();

      initEventsCommand.CommandText = "CREATE TABLE IF NOT EXISTS Events (Id TEXT, ActorName TEXT, EventIndex REAL, EventData TEXT)";
      initEventsCommand.ExecuteNonQuery();

      using var initSnapshotsCommand = connection.CreateCommand();

      initSnapshotsCommand.CommandText =
          "CREATE TABLE IF NOT EXISTS Snapshots (Id TEXT, ActorName TEXT, SnapshotIndex REAL, SnapshotData TEXT)";
      initSnapshotsCommand.ExecuteNonQuery();
    }

    private string ConnectionString => $"{_connectionStringBuilder}";

    public async Task DeleteEventsAsync(string actorName, long inclusiveToIndex)
    {
      using var connection = new SqliteConnection(ConnectionString);

      await connection.OpenAsync();

      using var deleteCommand = CreateCommand(
          connection,
          "DELETE FROM Events WHERE ActorName = $actorName AND EventIndex <= $inclusiveToIndex",
          ("$actorName", actorName),
          ("$inclusiveToIndex", inclusiveToIndex)
      );

      await deleteCommand.ExecuteNonQueryAsync();
    }

    public async Task DeleteSnapshotsAsync(string actorName, long inclusiveToIndex)
    {
      using var connection = new SqliteConnection(ConnectionString);

      await connection.OpenAsync();

      using var deleteCommand = CreateCommand(
          connection,
          "DELETE FROM Snapshots WHERE ActorName = $actorName AND SnapshotIndex <= $inclusiveToIndex",
          ("$actorName", actorName),
          ("$inclusiveToIndex", inclusiveToIndex)
      );

      await deleteCommand.ExecuteNonQueryAsync();
    }

    public async Task<long> GetEventsAsync(string actorName, long indexStart, long indexEnd, Action<object> callback)
    {
      using var connection = new SqliteConnection(ConnectionString);

      await connection.OpenAsync();

      using var selectCommand = CreateCommand(
          connection,
          "SELECT EventIndex, EventData FROM Events WHERE ActorName = $ActorName AND EventIndex >= $IndexStart AND EventIndex <= $IndexEnd ORDER BY EventIndex ASC",
          ("$ActorName", actorName),
          ("$IndexStart", indexStart),
          ("$IndexEnd", indexEnd)
      );

      var indexes = new List<long>();

      using var reader = await selectCommand.ExecuteReaderAsync();

      while (await reader.ReadAsync())
      {
        indexes.Add(Convert.ToInt64(reader["EventIndex"]));

        callback(JsonConvert.DeserializeObject<object>(reader["EventData"].ToString(), AutoTypeSettings));
      }

      return indexes.Any() ? indexes.LastOrDefault() : -1;
    }

    public async Task<(object Snapshot, long Index)> GetSnapshotAsync(string actorName)
    {
      object snapshot = null;
      long index = 0;

      using var connection = new SqliteConnection(ConnectionString);

      await connection.OpenAsync();

      using var selectCommand = CreateCommand(
          connection,
          "SELECT SnapshotIndex, SnapshotData FROM Snapshots WHERE ActorName = $ActorName ORDER BY SnapshotIndex DESC LIMIT 1",
          ("$ActorName", actorName)
      );

      using var reader = await selectCommand.ExecuteReaderAsync();

      while (await reader.ReadAsync())
      {
        snapshot = JsonConvert.DeserializeObject<object>(reader["SnapshotData"].ToString(), AutoTypeSettings);
        index = Convert.ToInt64(reader["SnapshotIndex"]);
      }

      return (snapshot, index);
    }

    public async Task<long> RecordEventAsync(string actorName, long index, object @event)
    {
      try
      {

      var item = new SqliteEvent(actorName, JsonConvert.SerializeObject(@event, AllTypeSettings), index);

      using var connection = new SqliteConnection(ConnectionString);

      await connection.OpenAsync();

      using var insertCommand = CreateCommand(
          connection,
          "INSERT INTO Events (Id, ActorName, EventIndex, EventData) VALUES ($Id, $ActorName, $EventIndex, $EventData)",
          ("$Id", item.Id),
          ("$ActorName", item.ActorName),
          ("$EventIndex", item.Index),
          ("$EventData", item.Json)
      );

      await insertCommand.ExecuteNonQueryAsync();

      return index++;
      }
      catch(Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        throw;
      }
    }

    public async Task RecordSnapshotAsync(string actorName, long index, object snapshot)
    {
      var item = new SqliteSnapshot(
          actorName, JsonConvert.SerializeObject(snapshot, AllTypeSettings), index
      );

      using var connection = new SqliteConnection(ConnectionString);

      await connection.OpenAsync();

      using var insertCommand = CreateCommand(
          connection,
          "INSERT INTO Snapshots (Id, ActorName, SnapshotIndex, SnapshotData) VALUES ($Id, $ActorName, $SnapshotIndex, $SnapshotData)",
          ("$Id", item.Id),
          ("$ActorName", item.ActorName),
          ("$SnapshotIndex", item.Index),
          ("$SnapshotData", item.Json)
      );

      await insertCommand.ExecuteNonQueryAsync();
    }

    private static SqliteCommand CreateCommand(SqliteConnection connection, string command, params (string Name, object Value)[] parameters)
    {
      var sqliteCommand = connection.CreateCommand();
      sqliteCommand.CommandText = command;
      sqliteCommand.Parameters.AddRange(parameters.Select(x => new SqliteParameter(x.Name, x.Value)));
      return sqliteCommand;
    }
  }
}
