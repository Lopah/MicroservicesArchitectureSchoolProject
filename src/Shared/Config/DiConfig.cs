using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApp.Shared.Config
{
    public static class DiConfig
    {
        public static void ConfigureDatabase<T>(this IServiceCollection services, string databaseName) where T : DbContext
        {
            services.AddDbContext<T>(options =>
                options.UseInMemoryDatabase(databaseName));
        }
    }
}
