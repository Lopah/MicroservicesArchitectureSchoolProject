using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoApp.Shared.Events.Users;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrdersService.Infrastructure.Data;

namespace OrdersService.Worker.Services.Users.UserDeletedEventConsumer
{
    public class UserDeletedEventConsumer : IConsumer<UserDeletedEvent>
    {
        private readonly ILogger<UserDeletedEventConsumer> _logger;
        private readonly ApplicationDbContext _context;

        public UserDeletedEventConsumer(ILogger<UserDeletedEventConsumer> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task Consume(ConsumeContext<UserDeletedEvent> context)
        {
            _logger.LogInformation($"Processing msg: '{context.MessageId}' with topic: '{context.ConversationId}'.");

            var user = await _context
                .OrderUsers
                .FirstOrDefaultAsync(e => e.Id == context.Message.Id);

            _context.Remove(user);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Deleted user with ID {user.Id} with name {user.Name}.");
        }
    }
}
