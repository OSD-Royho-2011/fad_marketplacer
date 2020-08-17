using NCB.Core.DataAccess.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.DataAccess.BaseRepository
{
    public interface IBaseRepository<T> : IBaseRepository<T, CatalogDbContext>
    {

    }
}
