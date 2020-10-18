﻿using DemoApp.Shared.Config;
using DemoApp.Shared.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductsService.Infrastructure.Data;
using ProductsService.Worker.Services.CreateProductConsumer;
using System;
using System.Collections.Generic;

namespace ProductsService.Worker
{
    public class Startup : IStartup
    {
        public void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;

            services.ConfigureDatabase<ApplicationDbContext>("Products");

            var rabbitOptions = new RabbitMqSettings();
            configuration.GetSection("RabbitOptions").Bind(rabbitOptions);

            var consumers = new List<Type> { typeof(CreateProductConsumer) };
            services.ConfigureRabbitMq(rabbitOptions, consumers);
        }
    }
}
