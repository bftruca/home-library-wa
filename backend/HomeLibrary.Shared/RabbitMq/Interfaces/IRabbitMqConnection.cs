using RabbitMQ.Client;

namespace HomeLibrary.Shared.RabbitMq.Interfaces
{
    public interface IRabbitMqConnection
    {
        Task<IConnection> GetConnectionAsync(
            CancellationToken cancellationToken = default);
    }
}
