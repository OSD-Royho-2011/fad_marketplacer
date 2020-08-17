using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Identity.Api.DataAccess.Entities
{
    [Table("AccountInRole")]
    public class AccountInRole : BaseEntity
    {
        public AccountInRole() : base()
        {

        }

        public Account Account { get; set; }

        public Guid AccountId { get; set; }

        public Role Role { get; set; }

        public Guid RoleId { get; set; }
    }
}
