using System;
using System.Collections.Generic;
using DemoApp.Shared.Config;
using DemoApp.Shared.Hosting;
using DemoApp.Worker.Services.OrderCreatedReceiver;
using DemoApp.Worker.Services.ProductCreatedConsumer;
using DemoApp.Worker.Services.UserCreatedConsumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoApp.Worker
{
    public class Startup : IStartup
    {
        public void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;

            var rabbitOptions = new RabbitMqSettings();
            configuration.GetSection("RabbitOptions").Bind(rabbitOptions);

            var consumers = new List<Type>
            {
                typeof(UserCreatedConsumer),
                typeof(ProductCreatedConsumer),
                typeof(OrderCreatedConsumer)
            };
            services.ConfigureRabbitMq(rabbitOptions, consumers);
        }
    }
}
