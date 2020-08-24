using NCB.Core.DataAccess.BaseUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Store.Api.DataAccess.BaseUnitOfWork
{
    public class UnitOfWork : BaseUnitOfWork<DataDbContext>, IUnitOfWork
    {
        public UnitOfWork(DataDbContext dbContext) : base(dbContext)
        {

        }
    }
}
