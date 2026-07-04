using HomeLibrary.Shared.Messages;
using HomeLibrary.Shared.RabbitMq.Interfaces;
using HomeLibrary.Shared.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace HomeLibrary.Shared.RabbitMq
{
    public sealed class RabbitMqConsumer : IRabbitMqConsumer
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RabbitMqOptions _options;

        public RabbitMqConsumer(
            IRabbitMqConnection rabbitMqConnection,
            IServiceScopeFactory scopeFactory,
            IOptions<RabbitMqOptions> options)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _scopeFactory = scopeFactory;
            _options = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
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

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += (_, args) =>
                ReceivedMessageAsync(channel, args, cancellationToken);

            await channel.BasicConsumeAsync(
                queue: _options.QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: cancellationToken);

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }

        private async Task ReceivedMessageAsync(
            IChannel channel,
            BasicDeliverEventArgs args,
            CancellationToken cancellationToken)
        {
            try
            {
                var json = Encoding.UTF8.GetString(args.Body.ToArray());

                var message = JsonSerializer.Deserialize<BookImportMessage>(json);

                if (message is null)
                {
                    await channel.BasicNackAsync(
                        args.DeliveryTag,
                        multiple: false,
                        requeue: false,
                        cancellationToken);

                    return;
                }

                using var scope = _scopeFactory.CreateScope();

                var repository = scope.ServiceProvider
                    .GetRequiredService<IBookRepository>();

                await repository.AddAsync(
                        message.ToEntity(),
                        cancellationToken);

                await channel.BasicAckAsync(
                    args.DeliveryTag,
                    multiple: false,
                    cancellationToken);
            }
            catch
            {
                await channel.BasicNackAsync(
                    args.DeliveryTag,
                    multiple: false,
                    requeue: true,
                    cancellationToken);
            }
        }
    }
}
