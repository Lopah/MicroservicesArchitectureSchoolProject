using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static void ConfigureRabbitMq(this IServiceCollection services, RabbitMqSettings settings, List<(string Endpoint, List<Type> ConsumerTypes)> consumers = null)
        {
            if(consumers is null)
                consumers = new List<(string endpoint, List<Type>)>();

            services.AddMassTransit(options =>
            {
                var allConsumerTypes = consumers.Select(c => c.ConsumerTypes).SelectMany(c => c.ToArray()).ToArray();
                options.AddConsumers(allConsumerTypes);

                options.AddBus(busContext =>
                {
                    var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(settings.Hostname, settings.Port, settings.VirtualHost, hostOptions =>
                        {
                            hostOptions.Username(settings.Username);
                            hostOptions.Password(settings.Password);
                        });

                        foreach (var consumer in consumers)
                        {
                            cfg.ReceiveEndpoint(consumer.Endpoint, ep =>
                            {
                                //Configure Rabbitmq exchange properties
                                ep.PrefetchCount = settings.PrefetchCount;

                                foreach (var consumerType in consumer.ConsumerTypes)
                                {
                                    ep.ConfigureConsumer(busContext, consumerType);
                                }
                            });
                        }

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
