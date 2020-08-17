using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCB.Core.DataAccess.BaseRepository
{
    public interface IBaseRepository<T, C>
    {
        //C GetDbContext();

        IQueryable<T> GetAll();

        void InsertAsync(T entity);

        void UpdateAsync(T entity);

        void DeleteAsync(Guid id);

        Task SaveAsync();
    }
}
