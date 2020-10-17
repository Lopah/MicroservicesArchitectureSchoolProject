using DemoApp.Infrastructure;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using UsersService.Infrastructure.Events;

namespace UsersService.Worker.Services.CreateUserReceiver
{
    public class CreateUserReceiverService : IHostedService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CreateUserReceiverService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task Handle(CreateUserEvent evt)
        {

        }
    }
}
