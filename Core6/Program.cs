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
                      // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
                      options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉
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

                // 讀取 XML 檔案產生 API 說明
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
        /// 設定 MS Sql
        /// </summary>
        /// <param name="builder"></param>
        private static void SetupMsSql(WebApplicationBuilder builder)
        {
            //設定MS SQL 
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