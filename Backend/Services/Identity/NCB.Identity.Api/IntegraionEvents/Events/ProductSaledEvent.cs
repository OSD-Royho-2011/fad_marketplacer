using NCB.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Identity.Api.IntegraionEvents.Events
{
    public class ProductSaledEvent : IntegrationEvent
    {
        public ProductSaledEvent() : base()
        {

        }

        public Guid ProductId { get; set; }
     
        public int Quantity { get; set; }
    }
}
