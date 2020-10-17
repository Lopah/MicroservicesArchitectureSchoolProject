using DemoApp.Shared.Hosting;
using Microsoft.Extensions.Hosting;

namespace OrdersService.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder()
                .UseStartup<Startup>();
    }
}
