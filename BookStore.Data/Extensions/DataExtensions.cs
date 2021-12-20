using BookStore.Application.Common.Interfaces;
using BookStore.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Data.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext(configuration);
    
    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<BookStoreDbContext>(c
                => c.UseSqlite(configuration.GetConnectionString("BookStore")));

        services
            .AddScoped<IBookStoreDbContext>(provider => provider.GetRequiredService<BookStoreDbContext>());

        return services;
    }
}