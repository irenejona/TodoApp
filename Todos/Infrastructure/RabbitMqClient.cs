using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Todos.Infrastructure
{
    public class RabbitMqClient : IRabbitMqClient
    {
        private readonly ILogger<RabbitMqClient> _logger;
        private readonly IModel _channel;

        public RabbitMqClient(ILogger<RabbitMqClient> logger)
        {
            _logger = logger;

            _logger.LogInformation("Starting RabbitMQClient");
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672,
                };
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
                _channel.ExchangeDeclare(exchange: Constants.ExchangeName, type: ExchangeType.Topic);
            }
            catch (Exception e)
            {
                _logger.LogError("RabbitMQClient failed to initialize: {Error}", JsonSerializer.Serialize(e));
                throw;
            }
        }

        public void PublishMessage(object message, string routingKey)
        {
            _logger.LogInformation("Publishing message to exchange with routing key {RoutingKey}", routingKey);

            var stringMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(stringMessage);
            _channel.BasicPublish(exchange: "todos",
                routingKey: routingKey,
                basicProperties: null,
                body: body);
            
            _logger.LogInformation("Message sent {Message}", stringMessage);
        }
    }
}