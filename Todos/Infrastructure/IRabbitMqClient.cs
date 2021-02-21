namespace Todos.Infrastructure
{
    public interface IRabbitMqClient
    {
        void PublishMessage(object message, string routingKey);
    }
}