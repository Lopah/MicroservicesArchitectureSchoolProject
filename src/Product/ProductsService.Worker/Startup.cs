using DemoApp.Shared.Config;
using DemoApp.Shared.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductsService.Infrastructure.Data;
using System;
using System.Collections.Generic;

using ProductServices = ProductsService.Worker.Services.Products;
using OrderServices = ProductsService.Worker.Services.Orders;

namespace ProductsService.Worker
{
    public class Startup : IStartup
    {
        public void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;

            var connectionString = configuration.GetConnectionString("Postgres");
            services.ConfigurePostgresDatabase<ApplicationDbContext>(connectionString);

            var rabbitOptions = new RabbitMqSettings();
            configuration.GetSection("RabbitOptions").Bind(rabbitOptions);

            var consumers = new List<Type> 
            { 
                typeof(ProductServices.CreateProductConsumer.CreateProductConsumer),
                typeof(ProductServices.EditProductConsumer.EditProductConsumer),
                typeof(ProductServices.DeleteProductConsumer.DeleteProductConsumer),
                typeof(OrderServices.OrderCreatedConsumer.OrderCreatedConsumer),
                typeof(OrderServices.OrderDeletedConsumer.OrderDeletedConsumer)
            };

            services.ConfigureRabbitMq(rabbitOptions, consumers);
        }
    }
}
