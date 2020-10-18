using DemoApp.Shared.Config;

namespace DemoApp.Core.Config
{
    public class AppSettings
    {
        public ServicesSettings Services { get; set; }
        public RabbitMqSettings RabbitOptions { get; set; }
    }

    public class ServicesSettings
    {
        public string UserServiceUrl { get; set; }
        public string ProductServiceUrl { get; set; }
        public string OrderServiceUrl { get; set; }
    }
}