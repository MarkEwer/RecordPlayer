using ME.RecordPlayer.EventSourcing;

namespace Paylocity.Surveys.Domain.Response
{
  public abstract class EntityBase
  {
    protected EntityBase(string id, Recorder recorder)
    {
      EntityId = id;
      Recorder = recorder;
    }
    protected string EntityId {get; set;}
    protected Recorder Recorder { get; set; }
  }
  public partial class ResultEntity { }
}
