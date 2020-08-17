using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCB.Identity.Api.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Identity.Api.DataAccess.EntityConfigurations
{
    public class AccountInRoleConfiguration : IEntityTypeConfiguration<AccountInRole>
    {
        public void Configure(EntityTypeBuilder<AccountInRole> builder)
        {
            builder.HasOne(u => u.Account)
                .WithMany(u => u.AccountInRoles)
                .HasForeignKey(pt => pt.AccountId);

            builder.HasOne(u => u.Role)
                .WithMany(u => u.AccountInRoles)
                .HasForeignKey(pt => pt.RoleId);
        }
    }
}
