using Microsoft.EntityFrameworkCore;
using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCB.Core.DataAccess.BaseRepository
{
    public class BaseRepository<T, C> : IBaseRepository<T, C>
        where T : BaseEntity
        where C : DbContext
    {
        private C _dbContext;

        private DbSet<T> _dbSet;

        public BaseRepository(C dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        //public func
        //public C GetDbContext()
        //{
        //    return _dbContext;
        //}

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public void InsertAsync(T entity)
        {
            _dbSet.Add(entity);
        }

        public void UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity).Property(x => x.RecordOrder).IsModified = false;
        }

        public void DeleteAsync(Guid id)
        {
            var entity = GetAll().FirstOrDefault(x => x.Id == id);
            _dbSet.Remove(entity);
        }

        // without Unit of work
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
