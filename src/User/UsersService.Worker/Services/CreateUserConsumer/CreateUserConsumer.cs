using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DemoApp.Shared.Events.Users;
using UsersService.Infrastructure.Data;
using UsersService.Infrastructure.Data.Entities;

namespace UsersService.Worker.Services.CreateUserConsumer
{
    public class CreateUserConsumer : IConsumer<CreateUserEvent>
    {
        private readonly ILogger<CreateUserConsumer> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateUserConsumer(ILogger<CreateUserConsumer> logger, ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<CreateUserEvent> context)
        {
            try
            {
                var user = context.Message;

                var dbUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = user.Username,
                    Name = user.Name,
                    Password = user.Password
                };

                await _dbContext.Users.AddAsync(dbUser);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Created user with ID {dbUser.Id} and username {dbUser.Username}");

                await _publishEndpoint.Publish(new UserCreatedEvent
                {
                    Id = dbUser.Id,
                    Name = dbUser.Name
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
