using HomeLibrary.Api.Services.Interfaces;
using HomeLibrary.Shared.RabbitMq.Interfaces;

namespace HomeLibrary.Api.Services;

public sealed class ImportService : IImportService
{
    private readonly ICsvParser _csvParser;
    private readonly IRabbitMqPublisher _publisher;

    public ImportService(
        ICsvParser csvParser,
        IRabbitMqPublisher publisher)
    {
        _csvParser = csvParser;
        _publisher = publisher;
    }

    public async Task<int> ImportAsync(
        Stream stream,
        CancellationToken cancellationToken = default)
    {
        var books = _csvParser.Parse(stream);

        if (books.Count == 0)
        {
            throw new InvalidDataException("CSV contains no valid rows.");
        }

        foreach (var book in books)
        {
            await _publisher.PublishAsync(book, cancellationToken);
        }

        return books.Count;
    }
}