using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.DataAccess.Entities
{
    [Table("Category")]
    public class Category : BaseEntity
    {
        public Category() : base()
        {

        }

        public string Name { get; set; }
    }
}
