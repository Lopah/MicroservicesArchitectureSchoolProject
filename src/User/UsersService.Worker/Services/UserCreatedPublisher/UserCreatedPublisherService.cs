using DemoApp.Infrastructure;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using UsersService.Infrastructure.Events;

namespace UsersService.Worker.Services.UserCreatedPublisher
{
    public class UserCreatedPublisherService : IHostedService
    {

        private const string TopicName = "UserCreated";
        private readonly ILogger<UserCreatedPublisherService> _logger;
        private IConnection _connection;
        private IModel _channel;

        public UserCreatedPublisherService(ILogger<UserCreatedPublisherService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var connectionFactory = new ConnectionFactory
            {
                UserName = "ops1",
                Password = "ops1",
                HostName = "localhost",
                Port = 5672
            };

            _connection = connectionFactory.CreateConnection( );
            _channel = _connection.CreateModel( );
            _channel.QueueDeclarePassive(TopicName);
            _channel.BasicQos(0, 1, false);


            _logger.LogInformation($"Queue [{TopicName}] is waiting for messages.");

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close( );
            _logger.LogInformation($"Service {nameof(UserCreatedPublisherService)} was stopped.");
            return Task.CompletedTask;
        }

        private async Task Handle(UserCreatedEvent evt)
        {
            try
            {
                var message = JsonSerializer.Serialize(evt);
                _channel.BasicPublish(message, TopicName);
                _logger.LogInformation("Sent message: ", message);
            }
            catch (AlreadyClosedException closedException)
            {
                _logger.LogError("RabbitMQ is closed! : ", closedException.ShutdownReason);
            }
        }
    }
}
