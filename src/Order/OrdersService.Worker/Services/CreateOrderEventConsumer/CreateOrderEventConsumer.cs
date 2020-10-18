using DemoApp.Shared.Events.Orders;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrdersService.Infrastructure.Data;
using OrdersService.Infrastructure.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersService.Worker.Services.CreateOrderEventConsumer
{
    public class CreateOrderEventConsumer : IConsumer<CreateOrderEvent>
    {
        private readonly ILogger<CreateOrderEventConsumer> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IPublishEndpoint _endpoint;

        public CreateOrderEventConsumer(ILogger<CreateOrderEventConsumer> logger, ApplicationDbContext context, IPublishEndpoint endpoint)
        {
            _logger = logger;
            _context = context;
            _endpoint = endpoint;
        }
        public async Task Consume(ConsumeContext<CreateOrderEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            var order = new Order
            {
                Id = new Guid(),
                OrderUser = await _context.OrderUsers
                    .FirstOrDefaultAsync(e => e.Id == context.Message.UserId),
                OrderProducts = await _context.OrderProducts
                    .Where(e => context.Message.ProductIds.Contains(e.Id))
                    .ToListAsync(),
            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Created user with ID {order.Id} for user {order.OrderUser.Name}");

            await _endpoint.Publish(new OrderCreatedEvent
            {
                Products = order.OrderProducts.Select(e => new OrderCreatedEvent.ProductDto
                {
                    Name = e.Name,
                    Price = e.Price,
                    Amount = e.Amount,
                    Id = e.Id
                }),
                UserId = order.OrderUser.Id,
                TotalPrice = order.OrderProducts.Sum(e => e.Amount),
                OrderId = order.Id
            });
        }
    }
}
