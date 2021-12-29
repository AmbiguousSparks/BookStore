using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using BookStore.Application.Authentication.Services;
using BookStore.Application.Cache.Config;
using BookStore.Application.Cache.Services;
using BookStore.Application.HealthChecks;
using BookStore.Application.Profiles;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Services;
using BookStore.Domain.Models.Users;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

[assembly: InternalsVisibleTo("BookStore.Application.Tests")]

namespace BookStore.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddRedisServices(configuration)
            .AddAutoMapper()
            .AddPipeLineBehaviors()
            .AddMediatR(config => config.AsScoped(), Assembly.GetExecutingAssembly())
            .AddFluentValidation();


    private static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(RedisConnection)).Get<RedisConnection>() 
                     ?? new RedisConnection
                     {
                         Host = "localhost",
                         Port = 6379
                     };

        services
            .AddScoped<ICacheService, RedisCacheService>()
            .AddSingleton<IDistributedCache>(x => new RedisCache(new RedisCacheOptions
            {
                Configuration = $"{config.Host}:{config.Port}"
            }))
            .TryAddSingleton(config);

        return services;
    }

    public static IHealthChecksBuilder AddApplicationChecks(this IHealthChecksBuilder builder) =>
        builder.AddCheck<RedisHealthCheck>("Redis");

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenConfig = configuration.GetSection(nameof(TokenConfiguration)).Get<TokenConfiguration>();

        var key = Encoding.UTF8.GetBytes(tokenConfig.SecretKey);

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = false,
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(tokenConfig);

        services.AddScoped<IAuthService, AuthService>();

        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddSingleton(tokenValidationParameters);

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwt =>
        {
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = tokenValidationParameters;
        });

        return services;
    }

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