using System.Threading.Tasks;
using DemoApp.Shared.Events.Users;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace DemoApp.Worker.Services.UserCreatedConsumer
{
    public class UserCreatedConsumer: IConsumer<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedConsumer> _logger;

        public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}'.");

            //TODO: Signal WEB project

            _logger.LogInformation($"Successfully processed msg: '{context.MessageId}'.");
        }
    }
}