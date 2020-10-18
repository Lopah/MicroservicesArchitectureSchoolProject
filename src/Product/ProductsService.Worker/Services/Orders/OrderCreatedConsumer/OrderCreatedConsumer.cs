using DemoApp.Shared.Events.Orders;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsService.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsService.Worker.Services.Orders.OrderCreatedConsumer
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            try
            {
                var order = context.Message;
                var productIds = order.Products.Select(x => x.Id);

                var products = await _applicationDbContext.Products.Where(x => productIds.Contains(x.Id)).ToListAsync();
                products.ForEach(x =>
                {
                    x.Amount -= order.Products.First(y => y.Id == x.Id).Amount;
                });

                await _applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(default, ex, ex.Message);
                throw;
            }
        }
    }
}
