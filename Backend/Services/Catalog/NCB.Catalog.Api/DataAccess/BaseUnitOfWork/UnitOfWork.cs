using NCB.Core.DataAccess.BaseUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.DataAccess.BaseUnitOfWork
{
    public class UnitOfWork : BaseUnitOfWork<CatalogDbContext>, IUnitOfWork
    {
        public UnitOfWork(CatalogDbContext dbContext) : base(dbContext)
        {

        }
    }
}
