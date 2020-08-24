using NCB.Store.Api.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Store.Api.DataAccess
{
    public class DataDbSeed
    {
        private DataDbContext _context;
        public DataDbSeed(DataDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.Cities.Any())
            {
                await _context.AddRangeAsync(CityData());
                await _context.SaveChangesAsync();
            }
        }

        public List<City> CityData()
        {
            return new List<City>()
            {
                    new City { Id = new Guid("ef64adfd-1afd-42c9-a76f-abb0add7956f"), Name = "City 1", Zipcode = 5000 },
                    new City { Id = new Guid("9e3fb378-d2ca-4e52-962c-d80d070c5b65"), Name = "City 2", Zipcode = 5001 },
                    new City { Id = new Guid("abfc058e-7e48-45b5-805c-93b0d0c7abd0"), Name = "City 3", Zipcode = 5002 }
            };
        }
    }
}
