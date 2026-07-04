using HomeLibrary.Shared.RabbitMq.Interfaces;

namespace HomeLibrary.Worker
{
    public sealed class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitMqConsumer _consumer;

        public Worker(
            ILogger<Worker> logger,
            IRabbitMqConsumer consumer)
        {
            _logger = logger;
            _consumer = consumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started.");

            return _consumer.StartAsync(stoppingToken);
        }
    }
}
