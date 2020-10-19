using DemoApp.Shared.Events.Products;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrdersService.Infrastructure.Data;
using System.Threading.Tasks;

namespace OrdersService.Worker.Services.Products.ProductEditedEventConsumer
{
    public class ProductEditedEventConsumer : IConsumer<ProductEditedEvent>
    {
        private readonly ILogger<ProductEditedEventConsumer> _logger;
        private readonly ApplicationDbContext _context;

        public ProductEditedEventConsumer(ILogger<ProductEditedEventConsumer> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task Consume(ConsumeContext<ProductEditedEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            var product = await _context.OrderProducts
                .FirstOrDefaultAsync(e => e.Id == context.Message.Id);

            product.Amount = context.Message.Amount;
            product.Price = context.Message.Price;
            product.Name = context.Message.Name;

            await _context.SaveChangesAsync( );

            _logger.LogInformation($"Edited user with ID {product.Id} with name: {product.Name}");
        }
    }
}
