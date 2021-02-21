using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notifications.Features;
using Notifications.Infrastructure;
using Serilog;
using ServicesTools;

namespace Notifications
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
                .AddMediatR(typeof(Startup))
                .AddMediatrBehaviours()
                .AddValidatorsFromAssembly(typeof(Startup).Assembly);

            services
                .AddHostedService<RabbitMqListener>()
                .AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            LogAppStartup(logger);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app
                .UseSerilogRequestLogging()
                .UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationsHub>("/subscribe");
            });
        }
        
        private void LogAppStartup(ILogger<Startup> logger)
        {
            logger.LogInformation("### STARTING NOTIFICATIONS-SERVICE ###");
        }
    }
}