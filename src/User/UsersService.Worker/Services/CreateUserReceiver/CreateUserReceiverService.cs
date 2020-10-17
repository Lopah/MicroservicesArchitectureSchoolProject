using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UsersService.Infrastructure.Data;
using UsersService.Infrastructure.Data.Entites;
using UsersService.Infrastructure.Events;

namespace UsersService.Worker.Services.CreateUserReceiver
{
    public class CreateUserReceiverService : IHostedService
    {
        private const string TopicName = "CreateUser";
        private readonly ILogger<CreateUserReceiverService> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private IConnection _connection;
        private IModel _channel;

        public CreateUserReceiverService(ILogger<CreateUserReceiverService> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var _connectionFactory = new ConnectionFactory
            {
                UserName = "ops1",
                Password = "ops1",
                DispatchConsumersAsync = true
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclarePassive(TopicName);
            _channel.BasicQos(0, 1, false);

            
            _logger.LogInformation($"Queue [{TopicName}] is waiting for messages.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            return Task.CompletedTask;
        }

        private Task Handle(CreateUserEvent evt)
        {

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (bc, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation($"Processing msg: '{message}'.");
                try
                {
                    var user = JsonSerializer.Deserialize<CreateUserEvent>(message);

                    var dbUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = user.Username,
                        Name = user.Name,
                        Password = user.Password
                    };

                    await _applicationDbContext.Users.AddAsync(dbUser);
                    await _applicationDbContext.SaveChangesAsync();

                    _logger.LogInformation($"Created user with ID {dbUser.Id} and username {dbUser.Username}");

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (JsonException)
                {
                    _logger.LogError($"JSON Parse Error: '{message}'.");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
                catch (AlreadyClosedException)
                {
                    _logger.LogInformation("RabbitMQ is closed!");
                }
                catch (Exception e)
                {
                    _logger.LogError(default, e, e.Message);
                }
            };

            _channel.BasicConsume(queue: TopicName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
