using HomeLibrary.Shared.Data;
using HomeLibrary.Shared.RabbitMq;
using HomeLibrary.Shared.RabbitMq.Interfaces;
using HomeLibrary.Shared.Repositories;
using HomeLibrary.Shared.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeLibrary.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDbContext<LibraryDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddRabbitMq(builder.Configuration);
            builder.Services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}
