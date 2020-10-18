using DemoApp.Shared.Events.Products;
using MassTransit;
using Microsoft.Extensions.Logging;
using ProductsService.Infrastructure.Data;
using ProductsService.Infrastructure.Data.Entities;
using System;
using System.Threading.Tasks;

namespace ProductsService.Worker.Services.CreateProductConsumer
{
    public class CreateProductConsumer : IConsumer<CreateProductEvent>
    {
        private readonly ILogger<CreateProductConsumer> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IPublishEndpoint publishEndpoint;

        public CreateProductConsumer(ILogger<CreateProductConsumer> logger, ApplicationDbContext applicationDbContext, IPublishEndpoint publishEndpoint)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<CreateProductEvent> context)
        {
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

                applicationDbContext.Products.Add(dbProduct);
                await applicationDbContext.SaveChangesAsync();

                logger.LogInformation($"Created product with ID {dbProduct.Id} and name {dbProduct.Name}");

                await publishEndpoint.Publish(new ProductCreatedEvent
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    Amount = dbProduct.Amount,
                    Price = dbProduct.Price
                });
            }
            catch (Exception ex)
            {
                logger.LogError(default, ex, ex.Message);
                throw;
            }
        }
    }
}
