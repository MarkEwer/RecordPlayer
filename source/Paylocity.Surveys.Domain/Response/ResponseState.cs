using Paylocity.Surveys.Domain.Template;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Paylocity.Surveys.Domain.Response
{
  public partial class ResponseState
  {
    private SortedDictionary<string, string> _answers = new();

    public Element Question { get; private set; }
    public string TextResponse { get; private set; }
    public string Comment { get; private set; }
    public IImmutableDictionary<string, string> Answers => _answers.ToImmutableDictionary();
  }
}
