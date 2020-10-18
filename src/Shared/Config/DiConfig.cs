using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DemoApp.Shared.Config
{
    public static class DiConfig
    {
        public static void ConfigureDatabase<T>(this IServiceCollection services, string databaseName) where T : DbContext
        {
            services.AddDbContext<T>(options =>
                options.UseInMemoryDatabase(databaseName));
        }

        public static void ConfigureRabbitMq(this IServiceCollection services, IConfigurationSection configuration, IEnumerable<Type> consumers)
        {
            services.AddMassTransit(options =>
            {
                var configSections = configuration.GetSection("Rabbitmq");
                var host = configSections["Host"];
                var userName = configSections["UserName"];
                var password = configSections["Password"];
                var virtualHost = configSections["VirtualHost"];
                var port = Convert.ToUInt16(configSections["Port"]);

                options.AddBus(busContext =>
                {
                    var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(host, port, virtualHost, hostOptions =>
                        {
                            hostOptions.Username(userName);
                            hostOptions.Password(password);
                        });

                        cfg.ReceiveEndpoint(configSections["Endpoint"], ep =>
                        {
                            //Configure Rabbitmq exchange properties
                            ep.PrefetchCount = Convert.ToUInt16(configSections["PrefetchCount"]);

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
