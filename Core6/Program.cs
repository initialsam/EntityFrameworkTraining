using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Core6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            SetupMsSql(builder);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        /// <summary>
        /// 設定 MS Sql
        /// </summary>
        /// <param name="builder"></param>
        private static void SetupMsSql(WebApplicationBuilder builder)
        {
            //設定MS SQL 
            var sqlConnection = "Server=.; Database=EFCore6_20230101; Trusted_Connection=true;Encrypt=false;";
            builder.Services.AddDbContext<MyContext>(
                opt => opt.UseSqlServer(
                    sqlConnection,
                    x => x.UseNetTopologySuite())
               .LogTo(Console.WriteLine,
                       new[] { DbLoggerCategory.Database.Command.Name },
                       LogLevel.Information)
               .EnableSensitiveDataLogging()
               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        }
    }


}