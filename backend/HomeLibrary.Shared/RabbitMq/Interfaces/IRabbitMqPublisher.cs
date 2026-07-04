using HomeLibrary.Shared.Messages;

namespace HomeLibrary.Shared.RabbitMq.Interfaces
{
    public interface IRabbitMqPublisher
    {
        Task PublishAsync(BookImportMessage message, CancellationToken cancellationToken = default);
    }
}
