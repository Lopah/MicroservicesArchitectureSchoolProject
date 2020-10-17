using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoApp.Infrastructure.Config.Rabbit.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DemoApp.Infrastructure.Config.Rabbit
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<Model>
    {
        public RabbitOptions Options { get; }

        public IConnection Connection => GetConnection();
        public RabbitModelPooledObjectPolicy(IOptions<RabbitOptions> options)
        {
            Options = options.Value;
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = Options.Hostname,
                UserName = Options.Username,
                Port = Options.Port,
                VirtualHost = Options.Host
            };

            return factory.CreateConnection();
        }

        public Model Create()
        {
            throw new NotImplementedException();
        }

        public bool Return(Model obj)
        {
            throw new NotImplementedException();
        }
    }
}
