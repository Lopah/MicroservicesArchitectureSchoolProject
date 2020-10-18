using System;
using System.Threading.Tasks;
using DemoApp.Core.Events;
using DemoApp.Shared.Events.Users;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UsersService.Infrastructure.Data;

namespace UsersService.Worker.Services.EditUserConsumer
{
    public class EditUserConsumer : IConsumer<EditUserEvent>
    {
        private readonly ILogger<EditUserConsumer> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public EditUserConsumer(ILogger<EditUserConsumer> logger, ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            this._logger = logger;
            this._dbContext = dbContext;
            this._publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<EditUserEvent> context)
        {
            try
            {
                var user = context.Message;

                if (!Guid.TryParse(user.Id, out Guid guid))
                {
                    throw new Exception("Failed to parse GUID");
                }

                var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == guid);

                dbUser.Name = user.Name;
                dbUser.Username = user.Username;
                dbUser.Password = user.Password;

                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Edited user with ID {dbUser.Id} and username {dbUser.Username}");

                await _publishEndpoint.Publish(new UserEditedEvent()
                {
                    Id = dbUser.Id,
                    Name = dbUser.Name,
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