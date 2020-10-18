using System.Threading.Tasks;
using DemoApp.Shared.Events;
using DemoApp.Shared.Events.Users;
using MassTransit;
using MassTransit.RabbitMqTransport.Integration;
using Microsoft.Extensions.Logging;

namespace DemoApp.Worker.Services.UserCreatePublisher
{
    public class UserCreatePublisher

    {
        private readonly ILogger<UserCreatePublisher> _logger;
        private readonly IPublishEndpoint _endpoint;

        public UserCreatePublisher(ILogger<UserCreatePublisher> logger, IPublishEndpoint endpoint)
        {
            _logger = logger;
            _endpoint = endpoint;
        }

        public async Task Publish(CreateUserEvent evt)
        {
            await _endpoint.Publish(evt);
            _logger.LogInformation("Sent message: {0}", evt.);
        }
    }
}
