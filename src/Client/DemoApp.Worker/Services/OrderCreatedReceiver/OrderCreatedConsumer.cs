using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DemoApp.Shared.Events.Orders;

namespace DemoApp.Worker.Services.OrderCreatedReceiver
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}'.");

            //TODO: Signal WEB project

            _logger.LogInformation($"Successfully processed msg: '{context.MessageId}'.");
        }
    }
}
