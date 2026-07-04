using HomeLibrary.Shared.Messages;
using HomeLibrary.Shared.RabbitMq.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace HomeLibrary.Shared.RabbitMq
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;
        private readonly RabbitMqOptions _options;

        public RabbitMqPublisher(
            IRabbitMqConnection rabbitMqConnection,
            IOptions<RabbitMqOptions> options
            )
        {
            _rabbitMqConnection = rabbitMqConnection;
            _options = options.Value;
        }

        public async Task PublishAsync(BookImportMessage message, CancellationToken cancellationToken = default)
        {
            var connection = await _rabbitMqConnection.GetConnectionAsync(cancellationToken);

            await using var channel = await connection.CreateChannelAsync(
                cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(
                queue: _options.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: cancellationToken);

            var body = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(message));

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: _options.QueueName,
                body: body,
                cancellationToken: cancellationToken);
        }
    }
}
