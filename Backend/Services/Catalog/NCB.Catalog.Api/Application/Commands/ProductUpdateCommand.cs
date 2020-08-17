using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.Application.Commands
{
    public class ProductUpdateCommand : IRequest<bool>
    {
        public ProductUpdateCommand()
        {

        }

        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
