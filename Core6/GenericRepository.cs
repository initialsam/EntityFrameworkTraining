using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using NetTopologySuite.Index.HPRtree;

namespace Core6;

public class GenericRepository<T> : IGenericRepository<T> where T : class, ISeqNo
{
    private IUnitOfWork UnitOfWork { get; }

    private readonly DbSet<T> _dbSet;

    public GenericRepository(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;

        _dbSet = UnitOfWork.MyContext.Set<T>();
    }

    public T Add(T entity)
    {
        var entityEntry = _dbSet.Add(entity);
        return entityEntry.Entity;
    }

    public async Task<T> AddAsync(T entity)
    {
        var entityEntry = await _dbSet.AddAsync(entity);
        return entityEntry.Entity;
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet.AsQueryable();
    }


    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        UnitOfWork.MyContext.Entry(entity).Property(x => x.SeqNo).IsModified = false;
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
        //還沒測試
        foreach (var entity in entities)
        {
            UnitOfWork.MyContext.Entry(entity).Property(x => x.SeqNo).IsModified = false;
        }

    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
    }

    public EntityEntry<T> Attach(T entity)
    {
        return _dbSet.Attach(entity);
    }

    public void AttachRemove(T entity)
    {
        var entityEntry = _dbSet.Attach(entity);
        entityEntry.State = EntityState.Deleted;
    }

    public async Task SaveAsync()
    {
        await UnitOfWork.SaveAsync();
    }
}
