using System.Reflection;
using BookStore.Application.Profiles;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddAutoMapper()
            .AddPipeLineBehaviors()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddFluentValidation();


    private static IServiceCollection AddPipeLineBehaviors(this IServiceCollection services)
    {
        var types = Assembly.GetExecutingAssembly().GetExportedTypes();

        var pipelines = types
            .Where(t =>
                t.GetInterfaces().Any(i => i.IsGenericType &&
                                           i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>)));

        foreach (var pipeline in pipelines)
            services.AddTransient(typeof(IPipelineBehavior<,>), pipeline);

        return services;
    }

    private static IServiceCollection AddAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

    private static IServiceCollection AddFluentValidation(this IServiceCollection services) =>
        services.AddFluentValidation(c =>
            c.RegisterValidatorsFromAssemblyContaining<BookStoreMapperProfile>());
}