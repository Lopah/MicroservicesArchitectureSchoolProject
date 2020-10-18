using DemoApp.Shared.Events.Products;
using MassTransit;
using Microsoft.Extensions.Logging;
using ProductsService.Infrastructure.Data;
using ProductsService.Infrastructure.Data.Entities;
using System;
using System.Threading.Tasks;

namespace ProductsService.Worker.Services.DeleteProductConsumer
{
    public class DeleteProductConsumer : IConsumer<DeleteProductEvent>
    {
        private readonly ILogger<DeleteProductConsumer> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteProductConsumer(ILogger<DeleteProductConsumer> logger, ApplicationDbContext applicationDbContext, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<DeleteProductEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            try
            {
                var productIdToDelete = context.Message.Id;
                _applicationDbContext.Products.Remove(new Product { Id = productIdToDelete });

                await _applicationDbContext.SaveChangesAsync();

                _logger.LogInformation($"Deleted product with ID {productIdToDelete}");

                await _publishEndpoint.Publish(new ProductDeletedEvent
                {
                    Id = productIdToDelete
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
