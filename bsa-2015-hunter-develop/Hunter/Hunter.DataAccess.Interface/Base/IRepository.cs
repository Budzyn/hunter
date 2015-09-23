using System;
using System.Linq;
using System.Linq.Expressions;
using Hunter.DataAccess.Entities;

namespace Hunter.DataAccess.Interface.Base
{
    public interface IRepository<T> where T : class, IEntity
    {
        IQueryable<T> Query();
        IQueryable<T> QueryIncluding(params Expression<Func<T, object>>[] includes);
        T Get(long id);
        T Get(Func<T, bool> predicate);
        void Update(T entity);
        void UpdateAndCommit(T entity);

        void Delete(T entity);
        void DeleteAndCommit(T entity);
    }
}