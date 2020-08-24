using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Store.Api.DataAccess.Entities
{
    [Table("Store")]
    public class Store : BaseEntity
    {
        public Store() : base()
        {

        }

        public string Name { get; set; }

        public string OpenTime { get; set; }

        public string CloseTime { get; set; }

        public string PriceRange { get; set; }

        //public Guid LocationId { get; set; }
    }
}
