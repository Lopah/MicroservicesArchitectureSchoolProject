using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoApp.Shared.Hosting
{
    public interface IStartup
    {
        void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services);
    }
}
