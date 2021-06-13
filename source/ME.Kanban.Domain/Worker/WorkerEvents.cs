using System;
using System.Collections.Generic;
using System.Linq;

namespace ME.Kanban.Domain.Worker
{
    public record BoardAccessGranted(string BoardId, string Role);

    public record Renamed(string GivenName, string Surname);

    public record AccountCanceled();
    public record Registered(string Name);
}
