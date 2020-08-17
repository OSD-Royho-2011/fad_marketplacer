using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NCB.Catalog.Api.DataAccess;
using NCB.Catalog.Api.DataAccess.BaseRepository;
using NCB.Catalog.Api.DataAccess.Entities;
using NCB.Catalog.Api.Services.Products.Commands;
using NCB.Catalog.Api.Services.Products.Models;
using NCB.Core.DataAccess.BaseRepository;
using NCB.Core.DataAccess.BaseUnitOfWork;
using NCB.Core.Extensions;
using NCB.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.Application.Queries
{
    public class ProductGetListQueryHandler : IRequestHandler<ProductGetListQuery, PagedList<ProductViewModel>>
    {
        private readonly IBaseRepository<Product> _productRepository;

        public ProductGetListQueryHandler(
            IBaseRepository<Product> productRepository
            )
        {
            _productRepository = productRepository;
        }

        public async Task<PagedList<ProductViewModel>> Handle(ProductGetListQuery request, CancellationToken cancellationToken)
        {
            var itemOnPage = await _productRepository
                .GetAll()
                .Include(x => x.Category)
                .PagedListAsync(request.Page ?? 1, request.Size ?? 10);

            var viewOnpPage = itemOnPage.Sources.Select(x => new ProductViewModel(x)).ToList();

            return new PagedList<ProductViewModel>(viewOnpPage, itemOnPage.Page, itemOnPage.Size, itemOnPage.TotalCount);
        }
    }
}
