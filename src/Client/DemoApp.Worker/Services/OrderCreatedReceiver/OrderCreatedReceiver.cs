using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DemoApp.Shared.Events.Orders;

namespace DemoApp.Worker.Services.OrderCreatedReceiver
{
    public class OrderCreatedReceiver : IConsumer<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedReceiver> _logger;

        public OrderCreatedReceiver(ILogger<OrderCreatedReceiver> logger)
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
