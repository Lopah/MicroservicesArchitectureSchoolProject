using Microsoft.Extensions.Hosting;

namespace DemoApp.Core.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseStartup<T>(this IHostBuilder builder) where T : IStartup, new()
        {
            var startup = new T();
            builder.ConfigureServices((hostContext, services) =>
            {
                startup.ConfigureServices(hostContext, services);
            });
            return builder;
        }
    }
}
