using Microsoft.EntityFrameworkCore;

namespace Core6;

public class MyContext : DbContext
{
    public MyContext(string connectionString) : base(
        new DbContextOptionsBuilder<DbContext>()
        .UseSqlServer(connectionString, x => x.UseNetTopologySuite())
        .Options)
    {

    }
    public MyContext(DbContextOptions<MyContext> options)
      : base(options)
    {
    }

    public DbSet<Product> Product { get; set; }
    

}
