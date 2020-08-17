using System;
using System.Collections.Generic;
using System.Text;

namespace NCB.EventBus.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            _id = Guid.NewGuid();
            _creationDate = DateTime.UtcNow;
        }

        public Guid _id { get; set; }

        public DateTime _creationDate { get; set; }
    }
}
