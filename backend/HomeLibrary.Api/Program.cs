
using HomeLibrary.Api.Services;
using HomeLibrary.Api.Services.Interfaces;
using HomeLibrary.Shared.Data;
using HomeLibrary.Shared.RabbitMq;
using HomeLibrary.Shared.RabbitMq.Interfaces;
using HomeLibrary.Shared.Repositories;
using HomeLibrary.Shared.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace HomeLibrary.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<LibraryDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IBookRepository, BookRepository>();

            builder.Services.AddRabbitMq(builder.Configuration);
            builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
            builder.Services.AddScoped<ICsvParser, CsvParser>();
            builder.Services.AddScoped<IImportService, ImportService>();
            builder.Services.AddScoped<IBookService, BookService>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
