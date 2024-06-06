using Application.Abstractions;
using Domain.Clients;
using Domain.Dishes;
using Domain.Menus;
using Domain.Orders;
using Infraestructure.BackgroundJobs;
using Infraestructure.Interceptors;
using Infraestructure.Repositories;
using Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using StackExchange.Redis;
using System.Collections.Concurrent;

namespace Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddSingleton<ICacheService, CacheService>(sp =>
        {
            var redis = sp.GetRequiredService<IConnectionMultiplexer>();
            var server = redis.GetServer(redis.GetEndPoints()[0]);
            IEnumerable<RedisKey> keys = server.Keys(pattern: "*");
            ConcurrentDictionary<string, bool> keyValues = new();
            foreach (RedisKey key in keys)
            {
                keyValues.TryAdd(key, false);
            }
            return new(sp.GetRequiredService<IDistributedCache>(), keyValues);
        });

        services.AddSingleton<OutboxInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, opt) =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("Database"))
                .AddInterceptors(sp.GetService<OutboxInterceptor>()!);
        });

        var redisConfigurationString = configuration.GetConnectionString("Redis");

        services.AddStackExchangeRedisCache(configure =>
        {
            configure.Configuration = redisConfigurationString;
        });

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfigurationString!));

        services.AddScoped<IDatabase>(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

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

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IDishRepository, DishRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
