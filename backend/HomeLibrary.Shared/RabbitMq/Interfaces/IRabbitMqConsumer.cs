using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLibrary.Shared.RabbitMq.Interfaces
{
    public interface IRabbitMqConsumer
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}
