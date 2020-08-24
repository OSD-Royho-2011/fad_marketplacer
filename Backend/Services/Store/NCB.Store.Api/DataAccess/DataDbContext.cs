using Microsoft.EntityFrameworkCore;
using NCB.Store.Api.DataAccess.Entities;
using NCB.Store.Api.DataAccess.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Store.Api.DataAccess
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {

        }

        public DbSet<Entities.Store> Stores { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Location> Locations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new StoreConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());
            //modelBuilder.ApplyConfiguration(new LocationConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
