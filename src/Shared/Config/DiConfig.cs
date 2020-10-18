using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DemoApp.Shared.Config
{
    public static class DiConfig
    {
        public static void ConfigureDatabase<T>(this IServiceCollection services, string databaseName) where T : DbContext
        {
            services.AddDbContext<T>(options =>
                options.UseInMemoryDatabase(databaseName));
        }

        public static void ConfigureRabbitMq<TConsumer, TMessage>(this IServiceCollection services, IConfigurationSection configuration) 
            where TConsumer : class, IConsumer<TMessage> where TMessage : class
        {
            services.AddMassTransit(x =>
            {
                var configSections = configuration.GetSection("Rabbitmq");
                var host = configSections["Host"];
                var userName = configSections["UserName"];
                var password = configSections["Password"];
                var virtualHost = configSections["VirtualHost"];
                var port = Convert.ToUInt16(configSections["Port"]);

                x.AddConsumer<TConsumer>();

                x.AddBus(provider =>
                {
                    var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(host, port, virtualHost, host =>
                        {
                            host.Username(userName);
                            host.Password(password);
                        });

                        cfg.ReceiveEndpoint(configSections["Endpoint"], ep =>
                        {
                            //Configure Rabbitmq exchange properties
                            ep.PrefetchCount = Convert.ToUInt16(configSections["PrefetchCount"]);

                            ep.ConfigureConsumer<TConsumer>(provider);
                        });
                    });

                    bus.Start();
                    
                    return bus;

                });
            });
        }
    }
}
