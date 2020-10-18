using MassTransit.RabbitMqTransport.Integration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using UsersService.Infrastructure.Events;

namespace UsersService.Worker.Services.UserCreatedPublisher
{
    public class UserCreatedPublisherService : IPublisher
    {

        private const string TopicName = "UserCreated";
        private readonly ILogger<UserCreatedPublisherService> _logger;
        private IConnection _connection;
        private IModel _channel;

        public UserCreatedPublisherService(ILogger<UserCreatedPublisherService> logger)
        {
            _logger = logger;
        }

        private Task Handle(UserCreatedEvent evt)
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

            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public Task Publish(string exchange, string routingKey, bool mandatory, IBasicProperties basicProperties, byte[] body,
            bool awaitAck)
        {
            throw new NotImplementedException();
        }
    }
}
