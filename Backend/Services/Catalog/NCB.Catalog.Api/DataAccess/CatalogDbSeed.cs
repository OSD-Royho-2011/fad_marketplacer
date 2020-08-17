using NCB.Catalog.Api.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.DataAccess
{
    public class CatalogDbSeed
    {
        private CatalogDbContext _context;
        public CatalogDbSeed(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.Categories.Any())
            {
                await _context.AddRangeAsync(CategoryData());
                await _context.SaveChangesAsync();
            }

            if (!_context.Products.Any())
            {
                await _context.AddRangeAsync(ProductData());
                await _context.SaveChangesAsync();
            }
        }

        public List<Category> CategoryData()
        {
            return new List<Category>()
            {
                new Category(){ Id = new Guid("ef64adfd-1afd-42c9-a76f-abb0add7956f"), Name = "Category 1" },
                new Category(){ Id = new Guid("9e3fb378-d2ca-4e52-962c-d80d070c5b65"), Name = "Category 2" },
                new Category(){ Id = new Guid("abfc058e-7e48-45b5-805c-93b0d0c7abd0"), Name = "Category 3" }
            };
        }

        public List<Product> ProductData()
        {
            return new List<Product>()
            {
                new Product(){ Id = new Guid("ba2b8f70-aa6a-4c1b-8d34-66a41c812edc"), Name = "Product 1", CategoryId = CategoryData()[0].Id, Price = 123.22M, Quantity = 10560 },
                new Product(){ Id = new Guid("5eceaa5e-80e0-4eab-8680-075de0b83db4"), Name = "Product 2", CategoryId = CategoryData()[1].Id, Price = 23, Quantity = 1002 },
                new Product(){ Id = new Guid("ffae9ab3-27ec-49f3-ba7c-26bea9c827e0"), Name = "Product 3", CategoryId = CategoryData()[2].Id, Price = 93.12M, Quantity = 9002 },
                new Product(){ Id = new Guid("6c0c6591-fdce-4f83-aa4e-90f537168609"), Name = "Product 4", CategoryId = CategoryData()[1].Id, Price = 0.33M, Quantity = 5108 },
                new Product(){ Id = new Guid("82728ac3-670a-4ef9-b600-11d38b521500"), Name = "Product 5", CategoryId = CategoryData()[0].Id, Price = 1.2M, Quantity = 1052 },
            };
        }
    }
}
