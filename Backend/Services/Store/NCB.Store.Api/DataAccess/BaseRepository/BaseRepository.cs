using NCB.Core.DataAccess.BaseRepository;
using NCB.Core.DataAccess.Entities;

namespace NCB.Store.Api.DataAccess.BaseRepository
{
    public class BaseRepository<T> : BaseRepository<T, DataDbContext>, IBaseRepository<T> where T : BaseEntity
    {
        public BaseRepository(DataDbContext dbContext) : base(dbContext)
        {

        }
    }
}
