using System;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using ServicesTools;
using Todos.Database;
using Todos.Infrastructure;

namespace Todos
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                opt.Filters.Add<HttpExceptionFilter>();
            });

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo {Title = "Todos", Version = "v1"});
                })
                .AddMediatR(typeof(Startup))
                .AddMediatrBehaviours()
                .AddValidatorsFromAssembly(typeof(Startup).Assembly)
                .AddAutoMapper(typeof(Startup))
                .AddSingleton<IRabbitMqClient, RabbitMqClient>();

            services.AddDbContext<TodosDbContext>(opt =>
                opt
                    .UseNpgsql("User ID=todos;Password=todos;Host=localhost;Port=5432;Database=todos;",
                        assembly =>
                        {
                            assembly.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                            assembly.SetPostgresVersion(new Version("11.6"));
                        })
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseSnakeCaseNamingConvention()
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            LogAppStartup(logger);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todos v1"));
            }
            
            app
                .UseSerilogRequestLogging()
                .UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void LogAppStartup(ILogger<Startup> logger)
        {
            logger.LogInformation("### STARTING TODOS-SERVICE ###");
        }
    }
}