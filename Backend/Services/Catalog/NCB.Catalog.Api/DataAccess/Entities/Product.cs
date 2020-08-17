using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.DataAccess.Entities
{
    [Table("Product")]
    public class Product : BaseEntity
    {
        public Product() : base()
        {
            Quantity = 0;
        }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
