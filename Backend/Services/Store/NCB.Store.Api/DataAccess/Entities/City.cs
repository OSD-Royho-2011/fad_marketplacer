using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Store.Api.DataAccess.Entities
{
    [Table("City")]
    public class City : BaseEntity
    {
        public City() : base()
        {

        }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Zipcode { get; set; }

        public List<Location> Locations { get; set; }
    }
}
