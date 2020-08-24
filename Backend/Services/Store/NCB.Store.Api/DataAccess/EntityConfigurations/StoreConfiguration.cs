using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Store.Api.DataAccess.EntityConfigurations
{
    public class StoreConfiguration : IEntityTypeConfiguration<Entities.Store>
    {
        public void Configure(EntityTypeBuilder<Entities.Store> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.HasData(
                new Entities.Store { Id = new Guid("ef64adfd-1afd-42c9-a76f-abb0add7956f"), Name = "Store 1", CloseTime = "6pm", OpenTime = "8am", PriceRange = "10000-300000" },
                new Entities.Store { Id = new Guid("9e3fb378-d2ca-4e52-962c-d80d070c5b65"), Name = "Store 2", CloseTime = "6pm", OpenTime = "8am", PriceRange = "10000-300000" },
                new Entities.Store { Id = new Guid("abfc058e-7e48-45b5-805c-93b0d0c7abd0"), Name = "Store 3", CloseTime = "6pm", OpenTime = "8am", PriceRange = "10000-300000" }
            );
        }
    }
}
