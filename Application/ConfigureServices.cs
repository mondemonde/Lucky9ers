using System.Reflection;
using FluentValidation;
using Lucky9.Application.Common.Behaviours;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection;
public static class ConfigureApplicsationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //keep this for erro logs
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
             
            //we are not husing this strategy 
            //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        });

        return services;
    }
}
