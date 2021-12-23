using BookStore.API.Common.Models.Config;

namespace BookStore.API.ApplicationStart;

public static class ApiServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddControllers();

        services.AddCors(opt => opt.AddPolicy("Default", builder =>
        {
            var corsConfig = configuration.GetSection(nameof(CorsConfig)).Get<CorsConfig>();

            builder.AllowCredentials();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
            builder.WithOrigins(corsConfig.CorsDomains.ToArray());
        }));

        return services;
    }
}