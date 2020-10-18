using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;

namespace DemoApp.Shared.Config
{
    public static class DiConfig
    {
        public static void ConfigurePostgresDatabase<T>(this IServiceCollection services, string connectionString) where T : DbContext
        {
            services.AddDbContext<T>(options =>
                options.UseNpgsql(connectionString));

            services.EnsureDatabaseIsCreated<T>();
        }

        public static void EnsureDatabaseIsCreated<T>(this IServiceCollection services) where T : DbContext
        {
            var serviceProvider = services.BuildServiceProvider();

            var dbContext = serviceProvider.GetService<T>();
            dbContext?.Database.EnsureCreated();
        }

        public static void ConfigureDatabase<T>(this IServiceCollection services, string databaseName) where T : DbContext
        {
            services.AddDbContext<T>(options =>
                options.UseInMemoryDatabase(databaseName));
        }

        public static void ConfigureRabbitMq(this IServiceCollection services, RabbitMqSettings settings, List<Type> consumers = null)
        {
            if(consumers is null)
                consumers = new List<Type>();

            services.AddMassTransit(options =>
            {
                foreach (var consumer in consumers)
                {
                    options.AddConsumer(consumer);
                }

                options.AddBus(busContext =>
                {
                    var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(settings.Hostname, settings.Port, settings.VirtualHost, hostOptions =>
                        {
                            hostOptions.Username(settings.Username);
                            hostOptions.Password(settings.Password);
                        });

                        cfg.ReceiveEndpoint(settings.Endpoint, ep =>
                        {
                            //Configure Rabbitmq exchange properties
                            ep.PrefetchCount = settings.PrefetchCount;

                            foreach (var consumer in consumers)
                            {
                                ep.ConfigureConsumer(busContext, consumer);
                            }
                        });
                    });

                    bus.Start();
                    
                    return bus;
                });
            });

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(2);
                options.Predicate = (check) => check.Tags.Contains("ready");
            });

            services.AddMassTransitHostedService();
        }
    }
}
