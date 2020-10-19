using DemoApp.Core.Config;
using DemoApp.Core.Services;
using DemoApp.Core.Services.Orders;
using DemoApp.Core.Services.Products;
using DemoApp.Core.Services.Users;
using DemoApp.Infrastructure;
using DemoApp.Shared.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoApp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews( )
                .AddRazorRuntimeCompilation();

            #region AppSettings

            var appSettings = new AppSettings();
            var section = Configuration.GetSection("AppSettings");
            section.Bind(appSettings);
            services.Configure<AppSettings>(section);

            #endregion

            #region Services

            services.AddHttpClient();
            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<IUserService,UserService>();

            #endregion

            services.ConfigureRabbitMq(appSettings.RabbitOptions);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment( ))
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseDeveloperExceptionPage( );
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts( );
            }
            app.UseHttpsRedirection( );
            app.UseStaticFiles( );

            app.UseRouting( );

            app.UseAuthorization( );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
