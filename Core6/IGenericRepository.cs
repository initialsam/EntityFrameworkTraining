using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Core6;
public interface ISeqNo
{
    int SeqNo { get; set; }
}
public interface IGenericRepository<T> where T : class, ISeqNo
{
    IQueryable<T> GetAll();

    IQueryable<T> Where(Expression<Func<T, bool>> expression);

    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

    T Add(T entity);

    Task<T> AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);

    EntityEntry<T> Attach(T entity);

    void Update(T entity);

    void UpdateRange(IEnumerable<T> entities);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);

    //IUnitOfWork UnitOfWork { get;}

    Task SaveAsync();
}