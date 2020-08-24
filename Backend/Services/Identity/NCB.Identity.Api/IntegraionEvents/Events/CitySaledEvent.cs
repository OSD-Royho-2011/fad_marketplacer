using NCB.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Identity.Api.IntegraionEvents.Events
{
    public class CitySaledEvent : IntegrationEvent
    {
        public CitySaledEvent() : base()
        {

        }


        public string Name { get; set; }

        public int Zipcode { get; set; }
    }
}
