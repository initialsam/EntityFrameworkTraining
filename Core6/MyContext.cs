using Microsoft.EntityFrameworkCore;

using System.Reflection;

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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //載入 Mapping 的所有設定
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public DbSet<Product> Product { get; set; }

    public void MigrateAndSeedData()
    {
        this.Database.Migrate();
    }
}
