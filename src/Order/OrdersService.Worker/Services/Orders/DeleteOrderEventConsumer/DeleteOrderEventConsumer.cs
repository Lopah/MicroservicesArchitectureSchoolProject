using DemoApp.Shared.Events.Orders;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrdersService.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersService.Worker.Services.Orders.DeleteOrderEventConsumer
{
    public class DeleteOrderEventConsumer : IConsumer<DeleteOrderEvent>
    {
        private readonly ILogger<DeleteOrderEventConsumer> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IPublishEndpoint _endpoint;

        public DeleteOrderEventConsumer(ILogger<DeleteOrderEventConsumer> logger, ApplicationDbContext context, IPublishEndpoint endpoint)
        {
            _logger = logger;
            _context = context;
            _endpoint = endpoint;
        }
        public async Task Consume(ConsumeContext<DeleteOrderEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(e => e.Id == context.Message.Id);

            _context.Remove(order);
            await _context.SaveChangesAsync( );

            _logger.LogInformation($"Deleted order with ID {order.Id} for user {order.OrderUser.Name}");

            await _endpoint.Publish(new OrderDeletedEvent
            {
                Products = order.OrderProducts.Select(e => new OrderDeletedEvent.ProductDto
                {
                    Amount = e.Amount,
                    Id = e.ProductId
                }).ToList(),
                Id = order.Id
            });
        }
    }
}
