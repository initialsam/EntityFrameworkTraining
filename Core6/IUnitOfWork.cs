namespace Core6;

public interface IUnitOfWork : IDisposable
{
    MyContext MyContext { get; }
    Task SaveAsync();
}