using System;
using System.Threading.Tasks;
using DemoApp.Shared.Events.Users;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UsersService.Infrastructure.Data;

namespace UsersService.Worker.Services.DeleteUserConsumer
{
    public class DeleteUserConsumer : IConsumer<DeleteUserEvent>
    {
        private readonly ILogger<DeleteUserConsumer> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteUserConsumer(ILogger<DeleteUserConsumer> logger, ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            this._logger = logger;
            this._dbContext = dbContext;
            this._publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<DeleteUserEvent> context)
        {
            try
            {
                var user = context.Message;

                var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

                _dbContext.Users.Remove(dbUser);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Deleted user with ID {dbUser.Id} and username {dbUser.Username}");

                await _publishEndpoint.Publish(new UserDeletedEvent()
                {
                    Id = dbUser.Id
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