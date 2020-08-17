using NCB.Core.DataAccess.BaseRepository;
using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.DataAccess.BaseRepository
{
    public class BaseRepository<T> : BaseRepository<T, CatalogDbContext>, IBaseRepository<T> where T : BaseEntity
    {
        public BaseRepository(CatalogDbContext dbContext) : base(dbContext)
        {

        }
    }
}
