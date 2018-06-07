using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Phony.Kernel.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int Id);
        TEntity Get(long Id);
        TEntity Get(string Name);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}