using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCB.Core.Models
{
    public class PagedList<TEntity>
    {
        public int Page { get; private set; }

        public int Size { get; private set; }

        public int TotalCount { get; private set; }

        public int TotalPages { get; private set; }

        public List<TEntity> Sources { get; set; }

        public bool HasPreviousPage
        {
            get { return (Page > 1); }
        }

        public bool HasNextPage
        {
            get { return (Page < TotalPages); }
        }

        //constructor
        public PagedList()
        {
            Sources = new List<TEntity>();
        }

        public PagedList(List<TEntity> sources, int page, int size, int total)
        {
            TotalCount = total;
            TotalPages = total / size;

            if (total % size > 0)
                TotalPages++;

            Size = size;
            Page = page;
            Sources = sources;
        }
    }

}
