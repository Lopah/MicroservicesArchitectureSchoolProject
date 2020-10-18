using DemoApp.Infrastructure;
using DemoApp.Infrastructure.SqlServer.DbEntities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Shared.Events.Orders;

namespace DemoApp.Worker.Services.OrderCreatedReceiver
{
    public class OrderCreatedReceiver : IConsumer<OrderCreatedEvent>
    {
        private const string TopicName = "OrderCreated";
        private readonly ILogger<OrderCreatedReceiver> _logger;
        private readonly ApplicationDbContext _dbContext;

        public OrderCreatedReceiver(ILogger<OrderCreatedReceiver> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}'.");

            var dbUser = await _dbContext.Users
                .Where(e => e.Id == context.Message.UserId)
                .FirstOrDefaultAsync( );

            var dbOrder = new Order
            {
                Products = context.Message.Products,
                Id = context.Message.OrderId,
                TotalPrice = context.Message.Products.Sum(x => x.Price)
            };

            dbUser.Orders.Add(dbOrder);

            await _dbContext.SaveChangesAsync( );

            _logger.LogInformation($"Created user with ID {dbUser.Id} and username {dbUser.Username}");
        }
    }
}
