using System.Threading;
using System.Threading.Tasks;
using DemoApp.Infrastructure;
using DemoApp.Infrastructure.SqlServer.DbEntities;
using Microsoft.Extensions.Hosting;
using DemoApp.Infrastructure.Events;
using RabbitMQ.Client;

namespace DemoApp.Worker.Services.UserCreatedReceiver
{
    public class UserCreatedReceiverService: IHostedService
    {
        private const string TopicName = "UserCreated";
        private readonly ApplicationDbContext _dbContext;
        private IConnection connection;
        private IModel model;

        public UserCreatedReceiverService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
            //Subscribe UserCreated topic -> Assign handle method
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(UserCreatedReceiverService)}");

            _connection.Close( );

            return Task.CompletedTask;
        }

        private async Task Handle(UserCreatedEvent evt)
        {
            var user = new User
            {
                Id = evt.Id,
                Name = evt.Name,
                Username = evt.Username,
                Password = evt.Password
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}