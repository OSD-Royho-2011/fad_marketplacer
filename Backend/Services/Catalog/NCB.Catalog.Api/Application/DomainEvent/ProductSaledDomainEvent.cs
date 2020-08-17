using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.Services.Products.Commands
{
    public class ProductSaledDomainEvent : INotification
    {
        public ProductSaledDomainEvent()
        {

        }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
