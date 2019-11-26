using EntityFrameworkTraining.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTraining.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public IUnitOfWork UnitOfWork { get; set; }

        private DbSet<T> DbSet { get; }

        public Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            DbSet = UnitOfWork.DataContext.Set<T>();
        }
        public IQueryable<T> LookupAll()
        {
            return DbSet.AsNoTracking();
        }
        //public IQueryable<T> LookupAllAsNoTracking()
        //{
        //    return DbSet.AsNoTracking();
        //}

        public IQueryable<T> Query(Expression<Func<T, bool>> filter)
        {
            return DbSet.Where(filter).AsNoTracking();
        }

        public IQueryable<T> QueryAsTracking(Expression<Func<T, bool>> filter)
        {
            return DbSet.Where(filter);
        }

        public T Create(T entity)
        {
            return DbSet.Add(entity);
        }

        public void Update(T entity)
        {
            DbSet.Attach(entity);
            UnitOfWork.DataContext.Entry(entity).State = EntityState.Modified;
        }
        public void Reload(T entity)
        {
            UnitOfWork.DataContext.Entry(entity).Reload();
        }
        public void Remove(T entity)
        {
            UnitOfWork.DataContext.Entry(entity).State = EntityState.Deleted;
        }

        public void Save()
        {
            UnitOfWork.Save();
        }

        public async Task SaveAsync()
        {
            await UnitOfWork.SaveAsync();
        }
    }
}
