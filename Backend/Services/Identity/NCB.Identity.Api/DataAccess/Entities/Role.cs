using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Identity.Api.DataAccess.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        public Role() : base()
        {
            
        }

        public string Name { get; set; }

        public List<AccountInRole> AccountInRoles { get; set; }
    }
}
