using DemoApp.Shared.Events.Users;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrdersService.Infrastructure.Data;
using System.Threading.Tasks;

namespace OrdersService.Worker.Services.Users.UserEditedEventConsumer
{
    public class UserEditedEventConsumer : IConsumer<UserEditedEvent>
    {
        private readonly ILogger<UserEditedEventConsumer> _logger;
        private readonly ApplicationDbContext _context;

        public UserEditedEventConsumer(ILogger<UserEditedEventConsumer> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task Consume(ConsumeContext<UserEditedEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            var user = await _context
                .OrderUsers
                .FirstOrDefaultAsync(e => e.Id == context.Message.Id);

            user.Name = context.Message.Name;

            await _context.SaveChangesAsync( );

            _logger.LogInformation($"Edited user with ID {user.Id} with name {user.Name}.");
        }
    }
}
