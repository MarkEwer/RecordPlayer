using System;
using System.Threading.Tasks;

namespace ME.RecordPlayer.EventSourcing
{
  public interface IEventStore
  {
    Task DeleteEventsAsync(string actorName, long inclusiveToIndex);

    Task<long> GetEventsAsync(string actorName, long indexStart, long indexEnd, Action<object> callback);

    Task<long> RecordEventAsync(string actorName, long index, object @event);
  }
}
