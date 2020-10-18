using DemoApp.Shared.Events.Users;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrdersService.Infrastructure.Data;
using OrdersService.Infrastructure.Data.Entities;
using System.Threading.Tasks;

namespace OrdersService.Worker.Services.UserCreatedEventConsumer
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedEventConsumer> _logger;
        private readonly ApplicationDbContext _context;

        public UserCreatedEventConsumer(ILogger<UserCreatedEventConsumer> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            var user = new OrderUser
            {
                Name = context.Message.Name,
                Id = context.Message.Id
            };

            _context.OrderUsers.Add(user);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Created user with ID {user.Id} with name: {user.Name}");
        }
    }
}
