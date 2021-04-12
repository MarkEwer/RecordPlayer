using System;

namespace ME.RecordPlayer.EventSourcing.Tests.SampleEntity
{
    public class Employee
    {
        public string AssignmentId { get; private set; }
        public Guid ClientId { get; private set; }
        public string CompanyId { get; private set; }
        public Guid EmployeeId { get; private set; }
        public string GivenName { get; private set; }
        public string PreferredName { get; private set; }
        public string Surname { get; private set; }

        public void ApplyEvent(object @event)
        {
            switch (@event)
            {
                case EmployeeAssigned evt:
                    Apply(evt);
                    break;
            }
        }

        private void Apply(EmployeeAssigned @event)
        {
            EmployeeId = @event.EmployeeId;
            GivenName = @event.GivenName;
            PreferredName = @event.PreferredName;
            Surname = @event.Surname;
            CompanyId = @event.CompanyId;
            AssignmentId = @event.AssignmentId;
            ClientId = @event.ClientId;
        }
    }
}
