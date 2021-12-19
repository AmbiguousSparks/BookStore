using System.Reflection;
using BookStore.Application.Pipelines;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddPipeLineBehaviors()
            .AddMediatR(Assembly.GetExecutingAssembly());
    
    
    private static IServiceCollection AddPipeLineBehaviors(this IServiceCollection services) =>
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggerPipeline<,>));
}