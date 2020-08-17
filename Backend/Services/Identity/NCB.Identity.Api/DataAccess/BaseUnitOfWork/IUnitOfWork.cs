using NCB.Core.DataAccess.BaseUnitOfWork;
using NCB.Identity.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Identity.Api.DataAccess.BaseUnitOfWork
{
    public interface IUnitOfWork : IBaseUnitOfWork<IdentityDbContext>
    {

    }
}
