using Microsoft.EntityFrameworkCore;
using NCB.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NCB.Core.Extensions
{
    public static class MigrationExtensions
    {
        public static void AddIfNotExist<TEntity>(
            this DbSet<TEntity> dbSet,
            Expression<Func<TEntity, object>> identifierExpression,
            params TEntity[] entities
            ) where TEntity : BaseEntity
        {
            var rest = entities.Where(x => !dbSet.Any(identifierExpression.ConvertExp(x)));

            dbSet.AddRange(rest);

            //foreach (var entity in entities)
            //{
            //    var v = identifierExpression.Compile()(entity);
            //    Expression<Func<TEntity, bool>> predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(Expression.Convert(identifierExpression.Body, v.GetType()), Expression.Constant(v)), identifierExpression.Parameters);

            //    var entry = dbSet.Any(predicate);
            //    if (!entry)
            //    {
            //        dbSet.Add(entity);
            //    }
            //}
        }

        public static void AddIfNotExist<TEntity>(this DbSet<TEntity> dbSet,
           List<Expression<Func<TEntity, object>>> identifierExpressions,
           params TEntity[] entities) where TEntity : BaseEntity
        {

            foreach (var entity in entities)
            {
                IQueryable<TEntity> entries = dbSet;
                foreach (var identifierExpression in identifierExpressions)
                {
                    var v = identifierExpression.Compile()(entity);
                    Expression<Func<TEntity, bool>> predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(Expression.Convert(identifierExpression.Body, v.GetType()), Expression.Constant(v)), identifierExpression.Parameters);
                    entries = entries.Where(predicate);
                }

                if (entries != null)
                {
                    var entry = entries.FirstOrDefault();
                    if (entry == null)
                    {
                        dbSet.Add(entity);
                    }
                }
            }
        }

        public static Expression<Func<TEntity, bool>> ConvertExp<TEntity>(this Expression<Func<TEntity, object>> pre, TEntity entity)
        {
            var value = pre.Compile()(entity);
            return Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(Expression.Convert(pre.Body, value.GetType()), Expression.Constant(value)), pre.Parameters);
        }
    }
}
