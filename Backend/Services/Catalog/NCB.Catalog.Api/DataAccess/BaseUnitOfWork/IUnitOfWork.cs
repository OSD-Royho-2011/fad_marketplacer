using NCB.Core.DataAccess.BaseUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.DataAccess.BaseUnitOfWork
{
    public interface IUnitOfWork : IBaseUnitOfWork<CatalogDbContext>
    {

    }
}
