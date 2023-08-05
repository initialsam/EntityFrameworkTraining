namespace Core6;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposedValue;

    public MyContext MyContext { get; private set; }

    public UnitOfWork(MyContext context)
    {
        MyContext = context;
    }

    public async Task SaveAsync()
    {
        await MyContext.SaveChangesAsync();
        MyContext.ChangeTracker.Clear();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                this.MyContext.Dispose();
                this.MyContext = null!;
            }
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}