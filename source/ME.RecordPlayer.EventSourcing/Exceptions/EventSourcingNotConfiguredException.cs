using System;
using System.Runtime.Serialization;

namespace ME.RecordPlayer.EventSourcing.Exceptions
{
    [Serializable()]
    public sealed class EventSourcingNotConfiguredException : System.Exception
    {
        public EventSourcingNotConfiguredException() : base()
        {
        }

        public EventSourcingNotConfiguredException(string message) : base(message)
        {
        }

        public EventSourcingNotConfiguredException(string message, System.Exception inner) : base(message, inner)
        {
        }

        private EventSourcingNotConfiguredException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
