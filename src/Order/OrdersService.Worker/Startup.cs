using DemoApp.Shared.Config;
using DemoApp.Shared.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrdersService.Infrastructure.Data;
using OrdersService.Worker.Services.CreateOrderEventConsumer;
using System;
using System.Collections.Generic;
using OrdersService.Worker.Services.ProductCreatedEventConsumer;
using OrdersService.Worker.Services.UserCreatedEventConsumer;

namespace OrdersService.Worker
{
    public class Startup : IStartup
    {
        internal IConfiguration Configuration { get; private set; }
        public void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            Configuration = hostContext.Configuration;

            services.ConfigureDatabase<ApplicationDbContext>("Orders");

            var rabbitOptions = new RabbitMqSettings( );
            Configuration.GetSection("RabbitOptions").Bind(rabbitOptions);

            var consumers = new List<Type> { typeof(CreateOrderEventConsumer), typeof(UserCreatedEventConsumer), typeof(ProductCreatedEventConsumer) };
            services.ConfigureRabbitMq(rabbitOptions, consumers);
        }
    }
}
