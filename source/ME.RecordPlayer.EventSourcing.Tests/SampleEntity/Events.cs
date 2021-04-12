using System;
using System.Collections.Generic;
using System.Linq;

namespace ME.RecordPlayer.EventSourcing.Tests.SampleEntity
{
    public record Uploaded(
        Guid DocumentId, string Filename, string DisplayName,
        DateTime UploadedDate, DateTime ReceivedDate,
        Guid ClientId, Guid User, DateTime EventDate);

    public record EmployeeAssigned(
        Guid DocumentId, Guid EmployeeId,
        string GivenName, string PreferredName, string Surname,
        string CompanyId, string AssignmentId,
        Guid ClientId, Guid User, DateTime EventDate);

    public record DisplayNameChanged(
        Guid DocumentId, string DisplayName,
        Guid ClientId, Guid User, DateTime EventDate);
}
