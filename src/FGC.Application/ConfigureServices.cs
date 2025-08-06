using FGC.Application.Common;
using FluentValidation;
using FGC.Application.Behaviours;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssemblyContaining<IApplicationDbContext>();
        services.AddMediatR(typeof(ApplicationAssemblyMarker).Assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>),
            typeof(UnhandledExceptionBehaviour<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>),
            typeof(ValidationBehaviour<,>));

        return services;
    }

}
public sealed class ApplicationAssemblyMarker { }



