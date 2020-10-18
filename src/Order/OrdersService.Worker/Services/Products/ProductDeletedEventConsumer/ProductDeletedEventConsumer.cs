using DemoApp.Shared.Events.Products;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrdersService.Infrastructure.Data;
using System.Threading.Tasks;

namespace OrdersService.Worker.Services.Products.ProductDeletedEventConsumer
{
    public class ProductDeletedEventConsumer : IConsumer<ProductDeletedEvent>
    {
        private readonly ILogger<ProductDeletedEventConsumer> _logger;
        private readonly ApplicationDbContext _context;

        public ProductDeletedEventConsumer(ILogger<ProductDeletedEventConsumer> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            var product = await _context.OrderProducts
                .FirstOrDefaultAsync(e => e.Id == context.Message.Id);

            _context.Remove(product);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Removed product with ID {product.Id} with name {product.Name}.");
        }
    }
}
