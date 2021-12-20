using System.Reflection;
using System.Runtime.CompilerServices;
using BookStore.Application.Common.Interfaces;
using BookStore.Data.Context;
using BookStore.Domain.Common.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: InternalsVisibleTo("BookStore.Data.Tests")]

namespace BookStore.Data.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext(configuration)
            .AddRepositories();

    internal static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<BookStoreDbContext>(c
                => c.UseSqlite(configuration.GetConnectionString("BookStore")));

        services
            .AddScoped<IBookStoreDbContext>(provider => provider.GetRequiredService<BookStoreDbContext>());

        return services;
    }

    internal static IServiceCollection AddAllRepositories(this IServiceCollection services) =>
        services
            .AddListRepositories()
            .AddPaginationRepositories()
            .AddRepositories();
            

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                !t.IsAbstract &&
                !t.IsInterface &&
                t.GetInterfaces().Any(i =>
                    i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(IRepository<>))
            );

        foreach (var type in types)
            services.AddScoped(typeof(IRepository<>), type);

        return services;
    }

    private static IServiceCollection AddListRepositories(this IServiceCollection services)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                !t.IsAbstract &&
                !t.IsInterface &&
                t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IListRepository<>))
            );

        foreach (var type in types)
            services.AddScoped(typeof(IListRepository<>), type);

        return services;
    }

    private static IServiceCollection AddPaginationRepositories(this IServiceCollection services)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                !t.IsAbstract &&
                !t.IsInterface &&
                t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IPaginationRepository<>))
            );

        foreach (var type in types)
            services.AddScoped(typeof(IPaginationRepository<>), type);

        return services;
    }
}