using DemoApp.Shared.Config;
using DemoApp.Shared.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UsersService.Infrastructure.Data;
using UsersService.Infrastructure.Events;
using UsersService.Worker.Services.CreateUserConsumer;

namespace UsersService.Worker
{
    public class Startup : IStartup
    {
        public void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;

            services.ConfigureDatabase<ApplicationDbContext>("Users");

            var rabbitConfig = configuration.GetSection("Rabbitmq");

            services.ConfigureRabbitMq<CreateUserConsumer, CreateUserEvent>(rabbitConfig);
        }
    }
}