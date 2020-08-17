using NCB.Core.DataAccess.Entities;
using NCB.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Identity.Api.DataAccess.Entities
{
    [Table("Account")]
    public class Account : BaseEntity
    {
        public Account() : base()
        {

        }

        public Account(string password) : base()
        {
            password.GeneratePassword(out string passwordSalt, out string passwordHash);

            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
        }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        public List<AccountInRole> AccountInRoles { get; set; }
    }
}
