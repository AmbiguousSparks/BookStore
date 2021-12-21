using System.Reflection;
using System.Runtime.CompilerServices;
using BookStore.Application.Profiles;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("BookStore.Application.Tests")]

namespace BookStore.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddAutoMapper()
            .AddPipeLineBehaviors()
            .AddMediatR(config => config.AsScoped(), Assembly.GetExecutingAssembly())
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
        services.AddAutoMapper(c => c.AddProfile<BookStoreMapperProfile>());

    private static IServiceCollection AddFluentValidation(this IServiceCollection services) =>
        services.AddFluentValidation(c =>
            c.RegisterValidatorsFromAssemblyContaining<BookStoreMapperProfile>());
}