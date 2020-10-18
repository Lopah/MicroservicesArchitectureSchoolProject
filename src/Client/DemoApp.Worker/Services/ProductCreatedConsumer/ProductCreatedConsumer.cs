using DemoApp.Shared.Events.Products;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DemoApp.Worker.Services.ProductCreatedConsumer
{
    public class ProductCreatedConsumer : IConsumer<ProductCreatedEvent>
    {
        private readonly ILogger<ProductCreatedConsumer> _logger;

        public ProductCreatedConsumer(ILogger<ProductCreatedConsumer> logger)
        {
            this._logger = logger;
        }

        public Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            var product = context.Message;

            _logger.LogInformation($"Received ProductCreatedEvent => ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Amount: {product.Amount}");

            return Task.CompletedTask;
        }
    }
}
