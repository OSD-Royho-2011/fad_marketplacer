using Microsoft.EntityFrameworkCore;
using NCB.Core.DataAccess.BaseRepository;
using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NCB.Core.DataAccess.BaseUnitOfWork
{
    public class BaseUnitOfWork<C> : IBaseUnitOfWork<C>
        where C : DbContext
    {
        private readonly C _context;

        public BaseUnitOfWork(C context)
        {
            _context = context;
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }

        //methods
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
