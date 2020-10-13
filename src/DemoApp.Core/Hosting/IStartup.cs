using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoApp.Core.Hosting
{
    public interface IStartup
    {
        void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services);
    }
}
