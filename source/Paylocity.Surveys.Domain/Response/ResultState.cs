using Paylocity.Surveys.Domain.Shared;
using System.Collections.Immutable;

namespace Paylocity.Surveys.Domain.Response
{
  public partial class ResultState 
  {
    private SortedSet<ResponseState> _responses = new();

    public long Id { get; private set; }
    public Person Participant { get; private set; }
    public string InstanceId { get; private set; }
    public IImmutableList<ResponseState> Responses => _responses.ToImmutableList();

  }
}
