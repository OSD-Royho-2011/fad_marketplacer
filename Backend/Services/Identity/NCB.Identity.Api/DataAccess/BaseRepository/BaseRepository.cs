using NCB.Core.DataAccess.BaseRepository;
using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Identity.Api.DataAccess.BaseRepository
{
    public class BaseRepository<T> : BaseRepository<T, IdentityDbContext>, IBaseRepository<T> where T : BaseEntity
    {
        public BaseRepository(IdentityDbContext dbContext) : base(dbContext)
        {

        }
    }
}
