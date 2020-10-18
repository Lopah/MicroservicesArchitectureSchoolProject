using DemoApp.Infrastructure;
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
using DemoApp.Infrastructure.SqlServer.DbEntities;
using DemoApp.Shared.Events.Products;

namespace DemoApp.Worker.Services.GetProductsReceiver
{
    public class GetProductsReceiver : IHostedService
    {
        private const string TopicName = "GetProductsResponse";
        private readonly ILogger<GetProductsReceiver> _logger;
        private readonly ApplicationDbContext _dbContext;
        private IConnection _connection;
        private IModel _model;

        public GetProductsReceiver(ILogger<GetProductsReceiver> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(GetProductsReceiver)}");
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

            _logger.LogInformation($"{nameof(GetProductsReceiver)} is now listening on topic {TopicName}.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(GetProductsReceiver)}");

            _connection.Close( );

            return Task.CompletedTask;
        }

        private Task Handle(ProductEvent evt)
        {
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (bc, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray( ));
                _logger.LogInformation($"Processing msg: '{message}'.");
                try
                {
                    var product = JsonSerializer.Deserialize<ProductEvent>(message);

                    var dbProduct = new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Amount = product.Amount,
                        Price = product.Price
                    };

                    await _dbContext.Products.AddAsync(dbProduct);
                    await _dbContext.SaveChangesAsync( );

                    _logger.LogInformation($"Created user with ID {dbProduct.Id} and name {dbProduct.Name}.");

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
