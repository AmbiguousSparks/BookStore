using BookStore.API.ApplicationStart;
using BookStore.API.Middlewares;
using BookStore.Application.Common.Interfaces;
using BookStore.Application.Extensions;
using BookStore.Data.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace BookStore.API;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services) =>
        services
            .AddData(Configuration)
            .AddApplication(Configuration)
            .AddAuthentication(Configuration)
            .AddApiServices(Configuration)
            .AddSingleton<TaskCanceledExceptionMiddleware>()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookStore", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Insert a valid Token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            })
            .AddLogging(l =>
            {
                l.AddConsole();
                l.AddFilter(f => f == LogLevel.Error || f == LogLevel.Warning);
            })
            .AddHealthChecks()
                .AddDataHealthCheck()
                .AddApplicationChecks();

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        MigrateDbToLatestVersion(app);
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            app.UseHsts();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore"));
        app.UseCors("Default");
        app.UseMiddleware<TaskCanceledExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseRouting(); 
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapHealthChecks("/health/simple");
            endpoints.MapHealthChecks("/health/extended", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }

    private static void MigrateDbToLatestVersion(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<IBookStoreDbContext>();

        if(context.Database.IsRelational())
            context.Database.Migrate();
    }
}