using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
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
            builder.Services
                  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddJwtBearer(options =>
                  {
                      // �����ҥ��ѮɡA�^�����Y�|�]�t WWW-Authenticate ���Y�A�o�̷|��ܥ��Ѫ��Բӿ��~��]
                      options.IncludeErrorDetails = true; // �w�]�Ȭ� true�A���ɷ|�S�O����
                      options.TokenValidationParameters = TokenHelper.CreateTokenValidation("TestApi", "this is key for your setting");

                      options.Events = new JwtBearerEvents
                      {
                          OnChallenge = context =>
                          {
                              context.HandleResponse();
                             
                              return context.Response.WriteAsync(context.ErrorDescription);
                          }

                      };
                  });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            SetupMsSql(builder);
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetRequiredService<MyContext>();
                dataContext.MigrateAndSeedData();
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        /// <summary>
        /// �]�w MS Sql
        /// </summary>
        /// <param name="builder"></param>
        private static void SetupMsSql(WebApplicationBuilder builder)
        {
            //�]�wMS SQL 
            var sqlConnection = "Server=(LocalDb)\\MSSQLLocalDB; Database=EFCore; Trusted_Connection=true;Encrypt=false;";
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