using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoApp.Shared.Events.Products;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrdersService.Infrastructure.Data;
using OrdersService.Infrastructure.Data.Entities;

namespace OrdersService.Worker.Services.ProductCreatedEventConsumer
{
    public class ProductCreatedEventConsumer: IConsumer<ProductCreatedEvent>
    {
        private readonly ILogger<ProductCreatedEventConsumer> _logger;
        private readonly ApplicationDbContext _context;

        public ProductCreatedEventConsumer(ILogger<ProductCreatedEventConsumer> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            var product = new OrderProduct
            {
                Name = context.Message.Name,
                Amount = context.Message.Amount,
                Price = context.Message.Price,
                Id = context.Message.Id
            };

            _context.OrderProducts.Add(product);

            _context.SaveChangesAsync();

            _logger.LogInformation($"Created product with ID {product.Id} with name {product.Name}.");

            return Task.CompletedTask;
        }
    }
}
