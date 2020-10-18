using DemoApp.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DemoApp.Worker.Services.GetProductsPublisher
{
    public class GetProductsPublisher : IHostedService
    {
        private const string TopicName = "GetProducts";
        private readonly ILogger<GetProductsPublisher> _logger;
        private readonly ApplicationDbContext _context;
        private IConnection _connection;
        private IModel _model;

        public GetProductsPublisher(ILogger<GetProductsPublisher> logger, ApplicationDbContext context)
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
                HostName = "localhost",
                Port = 5672
            };

            _connection = connectionFactory.CreateConnection( );
            _model = _connection.CreateModel( );
            _model.QueueDeclarePassive(TopicName);
            _model.BasicQos(0, 1, false);

            _logger.LogInformation($"{nameof(GetProductsPublisher)} is now listening on topic {TopicName}.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(GetProductsPublisher)}");

            _connection.Close( );
            return Task.CompletedTask;
        }

        public Task Handle(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                StopAsync(cancellationToken);
            }
            try
            {
                _model.BasicPublish("GetProducts", TopicName);
            }
            catch (AlreadyClosedException err)
            {
                _logger.LogError("RabbitMQ is closed! ", err.ShutdownReason);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled exception occurred: {0} , {1}", ex.Message, ex.StackTrace);
            }

            return Task.CompletedTask;
        }
    }
}
