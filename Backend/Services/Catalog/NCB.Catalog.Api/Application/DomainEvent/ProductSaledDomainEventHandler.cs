using MediatR;
using Microsoft.EntityFrameworkCore;
using NCB.Catalog.Api.DataAccess.BaseRepository;
using NCB.Catalog.Api.DataAccess.BaseUnitOfWork;
using NCB.Catalog.Api.DataAccess.Entities;
using NCB.Catalog.Api.Services.Products.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.Services.Products.Handlers
{
    public class ProductSaledDomainEventHandler : INotificationHandler<ProductSaledDomainEvent>
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductSaledDomainEventHandler(
            IBaseRepository<Product> productRepository,
            IUnitOfWork unitOfWork
            )
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProductSaledDomainEvent notification, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAll().Where(x => x.Id == notification.ProductId).FirstOrDefaultAsync();
            if (product != null)
            {
                product.Quantity -= notification.Quantity;

                _productRepository.UpdateAsync(product);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
