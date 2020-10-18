using DemoApp.Infrastructure;
using DemoApp.Infrastructure.SqlServer.DbEntities;
using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;

namespace DemoApp.Worker.Services.UserCreatedReceiver
{
    public class UserCreatedReceiverService: IConsumer<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedReceiverService> _logger;
        private readonly ApplicationDbContext _dbContext;

        public UserCreatedReceiverService(ILogger<UserCreatedReceiverService> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var user = new User
            {
                Id = context.Message.Id,
                Name = context.Message.Name,
                Username = context.Message.Username,
                Password = context.Message.Password
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync( );

            _logger.LogInformation($"Created user with ID {user.Id} and username {user.Username}");
        }
    }
}