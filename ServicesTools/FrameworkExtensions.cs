using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ServicesTools
{
    public static class FrameworkExtensions
    {
        public static IServiceCollection AddMediatrBehaviours(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehaviour<,>));

            return services;
        }
    }
}