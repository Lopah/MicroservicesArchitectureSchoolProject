using System.Threading;
using System.Threading.Tasks;
using DemoApp.Infrastructure;
using DemoApp.Infrastructure.SqlServer.DbEntities;
using Microsoft.Extensions.Hosting;
using DemoApp.Infrastructure.Events;

namespace DemoApp.Worker.Services.UserCreatedReceiver
{
    public class UserCreatedReceiverService: IHostedService
    {
        private readonly ApplicationDbContext _dbContext;

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
            return Task.CompletedTask;
            //Unsubscribe
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