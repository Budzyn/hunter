using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.DataAccess.Db.Base
{
    public abstract class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly DbContext _dataContext;

        protected Repository(IDatabaseFactory databaseFactory)
        {
            _dataContext = databaseFactory.Get();
        }

        protected DbContext DataContext
        {
            get { return _dataContext; }
        }

        public IQueryable<T> Query()
        {
            return _dataContext.Set<T>().AsQueryable();
        }

        public IQueryable<T> QueryIncluding(params Expression<Func<T, object>>[] includes)
        {
            var query = Query();
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }

        public T Get(long id)
        {
            return _dataContext.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public T Get(Func<T, bool> predicate)
        {
            return _dataContext.Set<T>().FirstOrDefault(predicate);
        }

        public void Delete(T entity)
        {
            var softDelete = entity as ISoftDeleteEntity;
            if (softDelete != null)
            {
                softDelete.IsDeleted = true;
                Update(entity);
            }
            else
            {
                _dataContext.Set<T>().Remove(entity);
            }
        }

        public void DeleteAndCommit(T entity)
        {
            Delete(entity);
            _dataContext.SaveChanges();
        }

        public void Update(T entity)
        {
            BeforeSave(entity);
            if (_dataContext.Entry(entity).State == EntityState.Detached)
            {
                _dataContext.Set<T>().Attach(entity);
                _dataContext.Entry(entity).State = entity.IsNew() ? EntityState.Added: EntityState.Modified;
            }
        }

        public void UpdateAndCommit(T entity)
        {
            Update(entity);
            _dataContext.SaveChanges();
        }


        /// <summary>
        /// Method is executed before calling Save and SaveAndCommit.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void BeforeSave(T entity)
        {
        }
    }
}