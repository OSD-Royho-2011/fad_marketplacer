using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NCB.Catalog.Api.Application.Queries;
using NCB.Catalog.Api.Controllers;
using NCB.Catalog.Api.DataAccess.BaseRepository;
using NCB.Catalog.Api.DataAccess.BaseUnitOfWork;
using NCB.Catalog.Api.DataAccess.Entities;
using NCB.Catalog.Api.Services.Products.Commands;
using NCB.Catalog.Api.Services.Products.Models;
using NCB.Core.Models;
using NCB.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.Api.UnitTests
{
    public class ProductControllerTest
    {
        private readonly Mock<ILogger<ProductsController>> _logger;
        private readonly Mock<IMediator> _mediator;

        public ProductControllerTest()
        {
            _logger = new Mock<ILogger<ProductsController>>();
            _mediator = new Mock<IMediator>();
        }

        //[Fact]
        //public async Task GetById_Return_Result()
        //{
        //    //Arrange
        //    //_productRepository.Setup(repo => repo.GetAll()).Returns(new AsyncEnumerableQuery<Product>(ProductData()));

        //    //Act
        //    var _productController = new ProductsController(
        //        _logger.Object,
        //        _mediator.Object
        //        );

        //    dynamic result = await _productController.GetById(1);

        //    //Assert
        //    Assert.Equal(5, result.Value.Data.Count);
        //    Assert.IsType<OkObjectResult>(result);
        //}

        [Fact]
        public async Task GetAll_Return_Result()
        {
            //Arrange
            _mediator.Setup(x => x.Send(It.IsAny<ProductGetListQuery>(), default(CancellationToken)))
                .Returns(Task.FromResult(new PagedList<ProductViewModel>()
                {
                    Sources = ProductData().Select(x => new ProductViewModel(x)).ToList()
                }));

            //Act
            var _productController = new ProductsController(
               _logger.Object,
               _mediator.Object
               );


            dynamic result = await _productController.GetAll(new ProductGetListQuery());

            //Assert
            Assert.Equal(5, result.Value.Data.Sources.Count);
            Assert.IsType<OkObjectResult>(result);
        }

        private List<Category> CategoryData()
        {
            return new List<Category>()
            {
                new Category(){ Id = new Guid("ef64adfd-1afd-42c9-a76f-abb0add7956f"), Name = "Category 1" },
                new Category(){ Id = new Guid("9e3fb378-d2ca-4e52-962c-d80d070c5b65"), Name = "Category 2" },
                new Category(){ Id = new Guid("abfc058e-7e48-45b5-805c-93b0d0c7abd0"), Name = "Category 3" }
            };
        }

        private List<Product> ProductData()
        {
            return new List<Product>()
            {
                new Product(){ Id = new Guid("ba2b8f70-aa6a-4c1b-8d34-66a41c812edc"), Name = "Product 1", Category = CategoryData()[0], Price = 123.22M },
                new Product(){ Id = new Guid("5eceaa5e-80e0-4eab-8680-075de0b83db4"), Name = "Product 2", Category = CategoryData()[1], Price = 23 },
                new Product(){ Id = new Guid("ffae9ab3-27ec-49f3-ba7c-26bea9c827e0"), Name = "Product 3", Category = CategoryData()[2], Price = 93.12M },
                new Product(){ Id = new Guid("6c0c6591-fdce-4f83-aa4e-90f537168609"), Name = "Product 4", Category = CategoryData()[1], Price = 0.33M },
                new Product(){ Id = new Guid("82728ac3-670a-4ef9-b600-11d38b521500"), Name = "Product 5", Category = CategoryData()[0], Price = 1.2M },
            };
        }
    }
}
