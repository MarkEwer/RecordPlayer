using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paylocity.Surveys.Domain.Shared
{
  public record class Person (Guid PersonId, string GivenName, string Surname, string? Position, string? CompanyId, string? EmployeeId);
}
