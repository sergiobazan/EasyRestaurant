using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Abstractions.Clock;
using Domain.Clients;
using Domain.Dishes;
using Domain.Menus;
using Domain.Orders;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Infraestructure.Authentication;
using Infraestructure.BackgroundJobs;
using Infraestructure.Interceptors;
using Infraestructure.Repositories;
using Infraestructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailService, EmailService>();

        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        AddPersistence(services, configuration);

        AddCacheWithRedis(services, configuration);

        AddBackgroundJob(services);

        AddAuthentication(services, configuration);

        return services;
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IIdentityUser, IdentityUser>();

        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("firebase.json")
        });

        services.AddHttpClient<IJwtService, JwtService>((sp, httpClient) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();

            httpClient.BaseAddress = new Uri(config["Authentication:TokenUri"]!);
        });

        services.Configure<JwtOptions>(configuration.GetSection("Authentication"));

        services.ConfigureOptions<JwtBearerConfigurationSetup>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddAuthorization();
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<OutboxInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, opt) =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("Database"))
                .AddInterceptors(sp.GetService<OutboxInterceptor>()!);
        });

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IDishRepository, DishRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }

    private static void AddCacheWithRedis(IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(configure =>
        {
            configure.Configuration = configuration.GetConnectionString("Redis");
        });

        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddBackgroundJob(IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure.AddJob<ProcessOutboxMessagesJob>(jobKey);
            configure.AddTrigger(trigger => 
                trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(10)
                            .RepeatForever()));
        });

        services.AddQuartzHostedService();
    }
}
