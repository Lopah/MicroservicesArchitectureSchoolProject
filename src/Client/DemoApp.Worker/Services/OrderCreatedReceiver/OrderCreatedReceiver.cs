using DemoApp.Infrastructure;
using DemoApp.Infrastructure.Events;
using DemoApp.Infrastructure.SqlServer.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DemoApp.Worker.Services.OrderCreatedReceiver
{
    public class OrderCreatedReceiver : IHostedService
    {
        private const string TopicName = "OrderCreated";
        private readonly ILogger<OrderCreatedReceiver> _logger;
        private readonly ApplicationDbContext _context;
        private IConnection _connection;
        private IModel _model;

        public OrderCreatedReceiver(ILogger<OrderCreatedReceiver> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var connectionFactory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                DispatchConsumersAsync = true,
                HostName = "localhost",
                Port = 5672
            };

            _connection = connectionFactory.CreateConnection( );
            _model = _connection.CreateModel( );
            _model.QueueDeclarePassive(TopicName);
            _model.BasicQos(0, 1, false);

            _logger.LogInformation($"{nameof(OrderCreatedReceiver)} is now listening on topic {TopicName}.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(OrderCreatedReceiver)}.");

            _connection.Close( );
            return Task.CompletedTask;
        }

        public Task Handle(OrderCreatedEvent evt, CancellationToken cancellationToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (bc, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray( ));
                _logger.LogInformation($"Processing msg: '{message}'.");
                try
                {
                    var orderReceived = JsonSerializer.Deserialize<OrderCreatedEvent>(message);

                    var dbUser = await _context.Users
                        .Where(e => e.Id == orderReceived.UserId)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                    var dbOrder = new Order
                    {
                        Products = orderReceived.Products,
                        Id = orderReceived.OrderId,
                        TotalPrice = orderReceived.Products.Sum(x => x.Price)
                    };

                    dbUser.Orders.Add(dbOrder);

                    await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation($"Created user with ID {dbUser.Id} and username {dbUser.Username}");

                    _model.BasicAck(ea.DeliveryTag, false);
                }
                catch (JsonException)
                {
                    _logger.LogError($"JSON Parse Error: '{message}'.");
                    _model.BasicNack(ea.DeliveryTag, false, false);
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

            return Task.CompletedTask;
        }
    }
}
