using Microsoft.EntityFrameworkCore;
using NCB.Core.DataAccess.BaseRepository;
using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NCB.Core.DataAccess.BaseUnitOfWork
{
    public interface IBaseUnitOfWork<C> : IDisposable
    {
        Task SaveAsync();
    }
}
