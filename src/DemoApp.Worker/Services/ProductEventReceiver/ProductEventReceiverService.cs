using System.Threading;
using System.Threading.Tasks;
using DemoApp.Worker.Services.ProductEventReceiver.EventDefinition;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DemoApp.Worker.Services.ProductEventReceiver
{
    public class ProductEventReceiverService : IHostedService
    {
        private readonly ILogger<ProductEventReceiverService> _logger;

        public ProductEventReceiverService(ILogger<ProductEventReceiverService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(ProductEventReceiverService)}");

            // subscribe to event bus, pass in options such as topic we want to push into bus and topic we want to consume

            _logger.LogInformation($"{nameof(ProductEventReceiverService)} is now listening.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(ProductEventReceiverService)}");

            // end unsubscribe from the event bus here
            
            return Task.CompletedTask;
        }

        private Task Handle(ProductEvent evt)
        {
            // repo/service call to save data.

            return Task.CompletedTask;
        }
    }
}
