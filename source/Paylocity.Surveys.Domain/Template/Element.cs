using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paylocity.Surveys.Domain.Template
{
  public abstract partial class Element
  {
    public long Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Type { get; private set; }
    public int PageNumber { get; private set; }
    public int SortOrder { get; private set; }
  }

  public partial class Header : Element { }
  public partial class TextQuestion : Element 
  { 
    public string Answer { get; private set; }
    public bool IsRequired { get; private set; }
  }
  public partial class SelectionQuestion : Element 
  { 
    public bool IsRequired { get; private set; }
    public bool AllowMultipleRespones { get; private set; }
    private readonly SortedDictionary<string, string> _answers = new();
    public IImmutableDictionary<string, string> Answers => _answers.ToImmutableDictionary();
  }
}
