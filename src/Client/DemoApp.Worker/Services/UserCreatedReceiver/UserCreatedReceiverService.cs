using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DemoApp.Shared.Events.Users;

namespace DemoApp.Worker.Services.UserCreatedReceiver
{
    public class UserCreatedReceiverService: IConsumer<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedReceiverService> _logger;

        public UserCreatedReceiverService(ILogger<UserCreatedReceiverService> logger)
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