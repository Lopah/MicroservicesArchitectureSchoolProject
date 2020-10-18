﻿using DemoApp.Infrastructure;
using DemoApp.Shared.Config;
using DemoApp.Shared.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoApp.Worker
{
    public class Startup : IStartup
    {
        public void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;

            services.ConfigureDatabase<ApplicationDbContext>("");

            // register event bus -- singleton

            // register core functionality
        }
    }
}