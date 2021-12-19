using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddPipeLineBehaviors()
            .AddMediatR(Assembly.GetExecutingAssembly());
    
    
    private static IServiceCollection AddPipeLineBehaviors(this IServiceCollection services)
    {
        var types = Assembly.GetExecutingAssembly().GetExportedTypes();

        foreach (var pipeline in types.Where(t => 
                     !t.IsAbstract && 
                     !t.IsInterface &&
                     t.IsAssignableTo(typeof(IPipelineBehavior<,>))))
            services.AddTransient(typeof(IPipelineBehavior<,>), pipeline);

        return services;
    }
}