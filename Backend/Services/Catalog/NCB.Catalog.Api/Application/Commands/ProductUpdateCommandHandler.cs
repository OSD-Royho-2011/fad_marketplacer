using MediatR;
using Microsoft.EntityFrameworkCore;
using NCB.Catalog.Api.DataAccess.BaseRepository;
using NCB.Catalog.Api.DataAccess.BaseUnitOfWork;
using NCB.Catalog.Api.DataAccess.Entities;
using NCB.Catalog.Api.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.Application.Commands
{
    public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, bool>
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductUpdateCommandHandler(
            IBaseRepository<Product> productRepository,
            IUnitOfWork unitOfWork
            )
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ProductUpdateCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAll().FirstOrDefaultAsync(x => x.Id == command.ProductId);

            if (product == null)
            {
                throw new CatalogException("product not found");
            }

            product.Quantity = command.Quantity;

            _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
