using System;
using System.Collections.Generic;
using System.Linq;

namespace ME.RecordPlayer.EventSourcing.Tests.SampleEntity
{
    public class DocumentState
    {
        public Guid ClientId { get; private set; }
        public string DisplayName { get; private set; }
        public Guid DocumentId { get; private set; }
        public string Filename { get; private set; }
        public Employee Owner { get; private set; }
        public DateTime Recieved { get; private set; }
        public DateTime Uploaded { get; private set; }

        public void ApplyEvent(object @event)
        {
            switch (@event)
            {
                case Uploaded evt:
                    Apply(evt);
                    break;

                case EmployeeAssigned evt:
                    if (Owner is null) Owner = new Employee();
                    Owner.ApplyEvent(evt);
                    break;

                case DisplayNameChanged evt:
                    Apply(evt);
                    break;
            }
        }

        private void Apply(Uploaded @event)
        {
            DocumentId = @event.DocumentId;
            Filename = @event.Filename;
            DisplayName = @event.DisplayName;
            Uploaded = @event.UploadedDate;
            Recieved = @event.ReceivedDate;
            ClientId = @event.ClientId;
        }

        private void Apply(DisplayNameChanged @event)
        {
            DisplayName = @event.DisplayName;
        }
    }
}
