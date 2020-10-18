namespace DemoApp.Shared.Config
{
    public class RabbitMqSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public ushort Port { get; set; }
        public string Hostname { get; set; }
        public string VirtualHost { get; set; }
        public ushort PrefetchCount { get; set; }
        public string Endpoint { get; set; }
    }
}