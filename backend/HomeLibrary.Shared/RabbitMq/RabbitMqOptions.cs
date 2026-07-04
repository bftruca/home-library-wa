namespace HomeLibrary.Shared.RabbitMq
{
    public sealed class RabbitMqOptions
    {
        public const string SectionName = "RabbitMq";

        public string HostName { get; init; } = default!;
        public int Port { get; init; }
        public string UserName { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string QueueName { get; init; } = default!;
    }
}
