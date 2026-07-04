using HomeLibrary.Shared.RabbitMq.Interfaces;

namespace HomeLibrary.Shared.RabbitMq
{
    internal class RabbitMqConsumer : IRabbitMqConsumer
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
