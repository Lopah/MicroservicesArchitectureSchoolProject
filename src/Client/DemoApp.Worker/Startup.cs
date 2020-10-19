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


            var consumers = new List<(string endpoint, List<Type>)>();

            var orderConsumers = new List<Type> {
                typeof(OrderCreatedConsumer)};
            consumers.Add(("orders", orderConsumers));

            var productConsumers = new List<Type> {
                typeof(ProductCreatedConsumer)};
            consumers.Add(("products", productConsumers));

            var userConsumers = new List<Type> {
                typeof(UserCreatedConsumer)};
            consumers.Add(("users", userConsumers));

            services.ConfigureRabbitMq(rabbitOptions, consumers);
        }
    }
}
