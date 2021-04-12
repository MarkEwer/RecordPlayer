using System;
using System.Collections.Generic;
using System.Linq;

namespace ME.RecordPlayer.EventSourcing.Tests.SampleEntity
{
    public record Upload(
        Guid DocumentId, string Filename, string DisplayName);

    public record AssignEmployee(
        Guid DocumentId, Guid EmployeeId,
        string GivenName, string PreferredName, string Surname,
        string CompanyId, string AssignmentId);

    public record RenameFile(
        Guid DocumentId, string DisplayName);

}
