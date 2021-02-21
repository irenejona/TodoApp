using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notifications.Features.TodoItemCreatedListener;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Notifications.Infrastructure
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly ILogger<RabbitMqListener> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IModel _channel;
        private readonly IConnection _connection;

        public RabbitMqListener(ILogger<RabbitMqListener> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            _logger.LogInformation("Starting RabbitMq Listener");
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672,
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: Constants.ExchangeName, type: ExchangeType.Topic);
            }
            catch (Exception ex)
            {
                _logger.LogError("RabbitListener initialization error, ex: {Message}", ex);
                throw;
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RegisterTodoItemsQueue();
            
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            
            return Task.CompletedTask;
        }

        private void RegisterTodoItemsQueue()
        {
            _logger.LogInformation("Registering to queue {QueueName}", Constants.TodoItemCreatedRoutingKey);
            
            _channel.QueueDeclare(queue:Constants.TodoItemCreatedRoutingKey, exclusive: false);
            _channel.QueueBind(queue: Constants.TodoItemCreatedRoutingKey,
                exchange: Constants.ExchangeName,
                routingKey: Constants.TodoItemCreatedRoutingKey);
            
            _logger.LogInformation("Waiting for messages");

            var todoItemConsumer = new EventingBasicConsumer(_channel);
            todoItemConsumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.Span);
                var result = await Process<TodoItemCreatedEvent>(message);
                if (result)
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            
            _channel.BasicConsume(queue: Constants.TodoItemCreatedRoutingKey, consumer: todoItemConsumer);
        }
        
        public async Task<bool> Process<T>(string message) where T : class
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            
            var @event = JsonSerializer.Deserialize<T>(message);
            if (@event is not null)
            {
                await mediator!.Send(@event);
            }

            return @event is null;
        }
    }
}