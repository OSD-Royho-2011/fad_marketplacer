using NCB.Core.DataAccess.BaseRepository;

namespace NCB.Store.Api.DataAccess.BaseRepository
{
    public interface IBaseRepository<T> : IBaseRepository<T, DataDbContext>
    {

    }
}
