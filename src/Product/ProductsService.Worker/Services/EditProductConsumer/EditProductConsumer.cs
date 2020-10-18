using DemoApp.Shared.Events.Products;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsService.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace ProductsService.Worker.Services.EditProductConsumer
{
    public class EditProductConsumer : IConsumer<EditProductEvent>
    {
        private readonly ILogger<EditProductConsumer> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public EditProductConsumer(ILogger<EditProductConsumer> logger, ApplicationDbContext applicationDbContext, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<EditProductEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            try
            {
                var product = context.Message;

                var dbProduct = await _applicationDbContext.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
                if (dbProduct == null)
                    throw new ApplicationException("Product not found");

                dbProduct.Name = product.Name;
                dbProduct.Price = product.Price;
                dbProduct.Amount = product.Amount;

                await _applicationDbContext.SaveChangesAsync();

                _logger.LogInformation($"Edited product with ID {dbProduct.Id} and name {dbProduct.Name}");

                await _publishEndpoint.Publish(new ProductEditedEvent
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
