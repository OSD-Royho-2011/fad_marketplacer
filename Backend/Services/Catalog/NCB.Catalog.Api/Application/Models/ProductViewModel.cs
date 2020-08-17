using NCB.Catalog.Api.DataAccess.Entities;
using NCB.Core.DataAccess.Entities;
using NCB.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.Services.Products.Models
{
    public class ProductViewModel : BaseViewModel
    {
        public ProductViewModel(Product product) : base(product)
        {
            if (product != null)
            {
                ProductId = product.Id;
                Price = product.Price;
                Quantity = product.Quantity;
                CategoryName = product.Category != null ? product.Category.Name : null;
            }
        }

        public Guid ProductId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string CategoryName { get; set; }
    }
}
