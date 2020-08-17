using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCB.Catalog.Api.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.DataAccess.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired(true)
                .HasMaxLength(100);

            // one-many auto setup
            //builder.HasOne(x => x.Category)
            //    .WithMany()
            //    .HasForeignKey(x => x.CategoryId);
        }
    }
}
