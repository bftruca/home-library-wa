using HomeLibrary.Shared.RabbitMq.Interfaces;
using RabbitMQ.Client;

namespace HomeLibrary.Shared.RabbitMq
{
    public sealed class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;

        public RabbitMqConnection(ConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IConnection> GetConnectionAsync(
            CancellationToken cancellationToken = default)
        {
            _connection ??= await _factory.CreateConnectionAsync(cancellationToken);

            return _connection;
        }
    }
}
