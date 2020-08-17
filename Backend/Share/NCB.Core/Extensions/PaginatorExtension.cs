using Microsoft.EntityFrameworkCore;
using NCB.Core.DataAccess.Entities;
using NCB.Core.Models;
using NCB.Core.Reflections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NCB.Core.Extensions
{
    public static class PaginatorExtension
    {
        public static async Task<PagedList<TSource>> PagedListAsync<TSource>(this IQueryable<TSource> source, int page, int size)
        {
            var _page = page.LimitToRange(1, null);
            var _size = size.LimitToRange(1, 50);

            var totalCount = await source.CountAsync();

            var sources = await source.Skip((_page - 1) * _size).Take(_size).ToListAsync();

            return new PagedList<TSource>(sources, _page, _size, totalCount);
        }

        //public static IOrderedQueryable<TSource> OrderByCondition<TSource>(this IQueryable<TSource> source, bool isDecs, string sortName) where TSource : BaseEntity
        //{
        //    //Expression<Func<TEntity, TKey>> keySelector;
        //    var properties = ReflectionUtilities.GetAllPropertyNamesOfType(typeof(TSource));

        //    return isDecs ? source.OrderByDescending(x => x.RecordOrder) : source.OrderBy(x => x.RecordOrder);
        //}

        public static int LimitToRange(this int value, int? min, int? max)
        {
            if (value < min && min.HasValue)
                return (int)min;
            if (value > max && max.HasValue)
                return (int)max;
            return value;
        }
    }
}
