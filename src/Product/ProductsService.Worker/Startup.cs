using DemoApp.Shared.Config;
using DemoApp.Shared.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductsService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using ProductsService.Worker.Services.Orders.OrderCreatedConsumer;
using ProductsService.Worker.Services.Orders.OrderDeletedConsumer;
using ProductsService.Worker.Services.Products.CreateProductConsumer;
using ProductsService.Worker.Services.Products.DeleteProductConsumer;
using ProductsService.Worker.Services.Products.EditProductConsumer;
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


            var consumers = new List<(string endpoint, List<Type>)>();

            var productConsumers = new List<Type> {
                typeof(CreateProductConsumer),
                typeof(EditProductConsumer),
                typeof(DeleteProductConsumer)};
            consumers.Add(("products", productConsumers));

            var orderConsumers = new List<Type> {
                typeof(OrderCreatedConsumer),
                typeof(OrderDeletedConsumer)};
            consumers.Add(("orders", orderConsumers));

            services.ConfigureRabbitMq(rabbitOptions, consumers);
        }
    }
}
