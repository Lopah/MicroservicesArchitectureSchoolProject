using DemoApp.Shared.Config;
using DemoApp.Shared.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrdersService.Infrastructure.Data;
using OrdersService.Worker.Services.Orders.CreateOrderEventConsumer;
using OrdersService.Worker.Services.Products.ProductCreatedEventConsumer;
using OrdersService.Worker.Services.Users.UserCreatedEventConsumer;
using System;
using System.Collections.Generic;
using OrdersService.Worker.Services.Orders.DeleteOrderEventConsumer;
using OrdersService.Worker.Services.Products.ProductDeletedEventConsumer;
using OrdersService.Worker.Services.Products.ProductEditedEventConsumer;
using OrdersService.Worker.Services.Users.UserDeletedEventConsumer;
using OrdersService.Worker.Services.Users.UserEditedEventConsumer;

namespace OrdersService.Worker
{
    public class Startup : IStartup
    {
        internal IConfiguration Configuration { get; private set; }
        public void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            Configuration = hostContext.Configuration;

            var connectionString = Configuration.GetConnectionString("Postgres");
            services.ConfigurePostgresDatabase<ApplicationDbContext>(connectionString);

            var rabbitOptions = new RabbitMqSettings( );
            Configuration.GetSection("RabbitOptions").Bind(rabbitOptions);

            var consumers = new List<Type>
            {
                typeof(CreateOrderEventConsumer),
                typeof(DeleteOrderEventConsumer),
                typeof(ProductCreatedEventConsumer),
                typeof(ProductEditedEventConsumer),
                typeof(ProductDeletedEventConsumer),
                typeof(UserCreatedEventConsumer),
                typeof(UserEditedEventConsumer),
                typeof(UserDeletedEventConsumer)
            };
            services.ConfigureRabbitMq(rabbitOptions, consumers);
        }
    }
}
