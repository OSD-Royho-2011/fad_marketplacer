using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Store.Api.DataAccess.Entities
{
    [Table("Location")]
    public class Location : BaseEntity
    {
        public Location() : base()
        {

        }
        public string Name { get; set; }

        public string Position { get; set; }

        public string Latiture { get; set; }

        public string Longitude { get; set; }

        public Guid CityId { get; set; }

        public List<Store> Stores { get; set; }
    }
}
