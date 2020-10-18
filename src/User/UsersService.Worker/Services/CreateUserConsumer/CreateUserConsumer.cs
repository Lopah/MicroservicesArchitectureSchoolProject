using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UsersService.Infrastructure.Data;
using UsersService.Infrastructure.Data.Entites;
using UsersService.Infrastructure.Events;

namespace UsersService.Worker.Services.CreateUserConsumer
{
    public class CreateUserConsumer : IConsumer<CreateUserEvent>
    {
        private readonly ILogger<CreateUserConsumer> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IPublishEndpoint publishEndpoint;

        public CreateUserConsumer(ILogger<CreateUserConsumer> logger, ApplicationDbContext applicationDbContext, IPublishEndpoint publishEndpoint)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.publishEndpoint = publishEndpoint;
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

                await applicationDbContext.Users.AddAsync(dbUser);
                await applicationDbContext.SaveChangesAsync();

                logger.LogInformation($"Created user with ID {dbUser.Id} and username {dbUser.Username}");

                await publishEndpoint.Publish(new UserCreatedEvent
                {
                    Id = dbUser.Id,
                    Username = dbUser.Username,
                    Name = dbUser.Name,
                    Password = dbUser.Password
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
