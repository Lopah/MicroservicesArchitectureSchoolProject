using System;
using System.Collections.Generic;
using DemoApp.Shared.Config;
using DemoApp.Shared.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UsersService.Infrastructure.Data;
using UsersService.Worker.Services.CreateUserConsumer;
using UsersService.Worker.Services.DeleteUserConsumer;
using UsersService.Worker.Services.EditUserConsumer;

namespace UsersService.Worker
{
    public class Startup : IStartup
    {
        internal IConfiguration Configuration { get; private set; }

        public void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            Configuration = hostContext.Configuration;

            var connectionString = Configuration.GetConnectionString("Postgres");
            services.ConfigurePostgresDatabase<ApplicationDbContext>(connectionString);

            var rabbitOptions = new RabbitMqSettings();
            Configuration.GetSection("RabbitOptions").Bind(rabbitOptions);

            var consumers = new List<Type> {typeof(CreateUserConsumer), typeof(EditUserConsumer), typeof(DeleteUserConsumer)};
            services.ConfigureRabbitMq(rabbitOptions, consumers);
        }
    }
}