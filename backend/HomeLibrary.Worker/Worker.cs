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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _consumer.StartAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "RabbitMQ consumer failed. Retrying in 5 seconds...");

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }

            _logger.LogInformation("Worker stopped.");
        }
    }
}
