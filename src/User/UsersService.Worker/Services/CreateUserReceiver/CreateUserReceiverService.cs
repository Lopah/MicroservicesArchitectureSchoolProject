﻿using Microsoft.Extensions.Hosting;
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
        private IModel _model;

        public CreateUserReceiverService(ILogger<CreateUserReceiverService> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
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


            _logger.LogInformation($"{nameof(CreateUserReceiverService)} is now listening on topic {TopicName}.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(CreateUserReceiverService)}");

            _connection.Close( );
            return Task.CompletedTask;
        }

        public Task Handle(CreateUserEvent evt)
        {

            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (bc, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray( ));
                _logger.LogInformation($"Processing msg: '{message}'.");
                try
                {
                    var user = JsonSerializer.Deserialize<CreateUserEvent>(message);

                    var dbUser = new User
                    {
                        Id = Guid.NewGuid( ),
                        Username = user.Username,
                        Name = user.Name,
                        Password = user.Password
                    };

                    await _applicationDbContext.Users.AddAsync(dbUser);
                    await _applicationDbContext.SaveChangesAsync( );

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

            _model.BasicConsume(queue: TopicName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
