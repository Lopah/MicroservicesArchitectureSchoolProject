using DemoApp.Shared.Events.Products;
using MassTransit;
using Microsoft.Extensions.Logging;
using ProductsService.Infrastructure.Data;
using ProductsService.Infrastructure.Data.Entities;
using System;
using System.Threading.Tasks;

namespace ProductsService.Worker.Services.Products.CreateProductConsumer
{
    public class CreateProductConsumer : IConsumer<CreateProductEvent>
    {
        private readonly ILogger<CreateProductConsumer> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateProductConsumer(ILogger<CreateProductConsumer> logger, ApplicationDbContext applicationDbContext, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<CreateProductEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            try
            {
                var product = context.Message;

                var dbProduct = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = product.Name,
                    Amount = product.Amount,
                    Price = product.Price
                };

                _applicationDbContext.Products.Add(dbProduct);
                await _applicationDbContext.SaveChangesAsync();

                _logger.LogInformation($"Created product with ID {dbProduct.Id} and name {dbProduct.Name}");

                await _publishEndpoint.Publish(new ProductCreatedEvent
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    Amount = dbProduct.Amount,
                    Price = dbProduct.Price
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(default, ex, ex.Message);
                throw;
            }
        }
    }
}
