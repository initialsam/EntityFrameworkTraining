using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;

using System.Reflection;

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
                             
                              return context.Response.WriteAsync(context.ErrorDescription ?? "No Error Description");
                          }

                      };
                  });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // Authorization
                options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization"
                });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                    });

                // Ū�� XML �ɮײ��� API ����
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //options.IncludeXmlComments(xmlPath);
            });

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