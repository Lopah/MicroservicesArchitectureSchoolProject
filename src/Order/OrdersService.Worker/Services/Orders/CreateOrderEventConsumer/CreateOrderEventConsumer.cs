using DemoApp.Shared.Events.Orders;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrdersService.Infrastructure.Data;
using OrdersService.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersService.Worker.Services.Orders.CreateOrderEventConsumer
{
    public class CreateOrderEventConsumer : IConsumer<CreateOrderEvent>
    {
        private readonly ILogger<CreateOrderEventConsumer> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IPublishEndpoint _endpoint;

        public CreateOrderEventConsumer(ILogger<CreateOrderEventConsumer> logger, ApplicationDbContext dbContext, IPublishEndpoint endpoint)
        {
            _logger = logger;
            _dbContext = dbContext;
            _endpoint = endpoint;
        }
        public async Task Consume(ConsumeContext<CreateOrderEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            var message = context.Message;
            var productIds = message.Products.Select(p => p.Id).ToList();

            var order = new Order
            {
                Id = new Guid( ),
                OrderUser = await _dbContext.OrderUsers
                    .FirstOrDefaultAsync(e => e.Id == message.UserId)
            };

            var products = await _dbContext.OrderProducts.Where(p => productIds.Contains(p.Id)).ToListAsync();
            var productLinks = new List<OrderProductLink>();
            foreach (var product in products)
            {
                var link = new OrderProductLink();
                link.Order = order;
                link.Product = product;
                link.Amount = message.Products.Where(p => p.Id == product.Id).Select(p => p.Amount).FirstOrDefault();

                await _dbContext.OrderProductLinks.AddAsync(link);
            }

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync( );

            _logger.LogInformation($"Created user with ID {order.Id} for user {order.OrderUser.Name}");

            await _endpoint.Publish(new OrderCreatedEvent
            {
                Products = order.OrderProducts.Select(e => new OrderCreatedEvent.ProductDto
                {
                    Amount = e.Amount,
                    Id = e.ProductId
                }),
                UserId = order.OrderUser.Id,
                TotalPrice = order.GetTotalPrice(),
                OrderId = order.Id
            });
        }
    }
}
