using MediatR;
using Microsoft.Extensions.Logging;
using NCB.Catalog.Api.DataAccess.BaseRepository;
using NCB.Catalog.Api.DataAccess.BaseUnitOfWork;
using NCB.Catalog.Api.DataAccess.Entities;
using NCB.Catalog.Api.IntegrationEvents.Events;
using NCB.Catalog.Api.Services.Products.Commands;
using NCB.EventBus.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.IntegrationEvents.EventHandlers
{
    public class ProductSaledEventHandler : IEventHandler
    {
        private readonly ILogger<ProductSaledEventHandler> _logger;
        private readonly IMediator _mediator;

        public ProductSaledEventHandler(
            ILogger<ProductSaledEventHandler> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(dynamic eventData)
        {
            try
            {
                var ev = (ProductSaledEvent)JsonConvert.DeserializeObject(eventData.ToString(), typeof(ProductSaledEvent));

                await _mediator.Publish(new ProductSaledDomainEvent() { ProductId = ev.ProductId, Quantity = ev.Quantity});

                _logger.LogInformation($"log from 'ProductSaledEventHandler': {ev._id}");
            }
            catch (Exception e)
            {
                throw e;
            }

            await Task.CompletedTask;
        }
    }
}
