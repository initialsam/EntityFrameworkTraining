using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTraining.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// unit of work
        /// </summary>
        IUnitOfWork UnitOfWork { get; set; }
        /// <summary>
        /// Lookups all.
        /// </summary>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        IQueryable<T> LookupAll();
        /// <summary>
        /// 搜尋
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Query(Expression<Func<T, bool>> filter);
        /// <summary>
        /// 搜尋 AsTacking
        /// </summary>
        /// <returns></returns>
        IQueryable<T> QueryAsTracking(Expression<Func<T, bool>> filter);
        ///// <summary>
        ///// Lookups all. AsNoTracking
        ///// </summary>
        ///// <returns>IQueryable&lt;T&gt;.</returns>
        //IQueryable<T> LookupAllAsNoTracking();
        ///// <summary>
        ///// 搜尋 AsNoTracking
        ///// </summary>
        ///// <returns></returns>
        //IQueryable<T> QueryAsNoTracking(Expression<Func<T, bool>> filter);
        ///// <summary>
        ///// 取得單一 entity
        ///// </summary>
        ///// <param name="filter"></param>
        ///// <returns></returns>
        //T GetSingle(Expression<Func<T, bool>> filter);
        /// <summary>
        /// 新增一個entity
        /// </summary>
        /// <param name="entity"></param>
        T Create(T entity);
        /// <summary>
        /// 更新單一entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
        /// <summary>
        /// 刪除單一entity
        /// </summary>
        /// <param name="entity"></param>
        void Remove(T entity);

        /// <summary>
        /// save change
        /// </summary>
        void Save();
        /// <summary>
        /// async save change
        /// </summary>
        Task SaveAsync();

        void Reload(T entity);

    }
}
