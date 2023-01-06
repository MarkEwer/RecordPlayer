using LiteDB;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ME.RecordPlayer.EventSourcing.LiteDb
{
  public class LiteDbProvider : IProvider
  {
#pragma warning disable SCS0028 // TypeNameHandling is set to other value than 'None' that may lead to deserialization vulnerability
    private static readonly JsonSerializerSettings AllTypeSettings = new() { TypeNameHandling = TypeNameHandling.All };
    private static readonly JsonSerializerSettings AutoTypeSettings = new() { TypeNameHandling = TypeNameHandling.Auto };
#pragma warning restore SCS0028 // TypeNameHandling is set to other value than 'None' that may lead to deserialization vulnerability
    private readonly string _connectionString;

    public LiteDbProvider(string connectionString)
    {
      _connectionString = connectionString;
      using var db = new LiteDatabase(_connectionString);
      var events = db.GetCollection<LiteDbEvent>();
      _ = events.EnsureIndex(x => x.ActorName);
      _ = events.EnsureIndex(x => x.Index);

      var snapshots = db.GetCollection<LiteDbSnapshot>();
      _ = snapshots.EnsureIndex(x => x.ActorName);
      _ = snapshots.EnsureIndex(x => x.Index);
    }

    public Task DeleteEventsAsync(string actorName, long inclusiveToIndex)
    {
      try
      {
        using var db = new LiteDatabase(_connectionString);
        var events = db.GetCollection<LiteDbEvent>();
        return Task.FromResult(events.DeleteMany(x => x.ActorName == actorName && x.Index <= inclusiveToIndex));
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        throw;
      }
    }

    public Task DeleteSnapshotsAsync(string actorName, long inclusiveToIndex)
    {
      try
      {
        using var db = new LiteDatabase(_connectionString);
        var snapshots = db.GetCollection<LiteDbSnapshot>();
        return Task.FromResult(snapshots.DeleteMany(x => x.ActorName == actorName && x.Index <= inclusiveToIndex));
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        throw;
      }
    }

    public Task<long> GetEventsAsync(string actorName, long indexStart, long indexEnd, Action<object> callback)
    {
      try
      {
        using var db = new LiteDatabase(_connectionString);
        var events = db.GetCollection<LiteDbEvent>();
        long lastIndex = 0L;

        var found = events.Find(x => x.ActorName == actorName && x.Index >= indexStart && x.Index <= indexEnd)
                          .OrderBy(x => x.Index);
        foreach (var @event in found)
        {
          lastIndex = @event.Index;
          callback(JsonConvert.DeserializeObject<object>(@event.Json, AutoTypeSettings));
        }
        return Task.FromResult(lastIndex);
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        throw;
      }
    }

    public Task<(object Snapshot, long Index)> GetSnapshotAsync(string actorName)
    {
      try
      {
        using var db = new LiteDatabase(_connectionString);
        var snapshots = db.GetCollection<LiteDbSnapshot>();

        var found = snapshots.Find(x => x.ActorName == actorName)
                          .OrderByDescending(x => x.Index)
                          .FirstOrDefault();

        object snapshot = JsonConvert.DeserializeObject<object>(found.Json, AutoTypeSettings);
        return Task.FromResult((snapshot, found.Index));
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        throw;
      }
    }

    public Task<long> RecordEventAsync(string actorName, long index, object @event)
    {
      try
      {
        using var db = new LiteDatabase(_connectionString);
        var events = db.GetCollection<LiteDbEvent>();
        _ = events.Insert(new LiteDbEvent(actorName, JsonConvert.SerializeObject(@event, AllTypeSettings), index));
        return Task.FromResult(index + 1);
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        throw;
      }
    }

    public Task RecordSnapshotAsync(string actorName, long index, object snapshot)
    {
      try
      {
        using var db = new LiteDatabase(_connectionString);
        var snapshots = db.GetCollection<LiteDbSnapshot>();
        _ = snapshots.Insert(new LiteDbSnapshot(actorName, JsonConvert.SerializeObject(snapshot, AllTypeSettings), index));
        return Task.FromResult(index + 1);
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        throw;
      }
    }
  }
}
