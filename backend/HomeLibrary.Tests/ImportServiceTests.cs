using HomeLibrary.Api.Services;
using HomeLibrary.Api.Services.Interfaces;
using HomeLibrary.Shared.Messages;
using HomeLibrary.Shared.RabbitMq.Interfaces;
using Moq;

namespace HomeLibrary.Tests;

public class ImportServiceTests
{
    private readonly Mock<ICsvParser> _csvParserMock;
    private readonly Mock<IRabbitMqPublisher> _publisherMock;

    private readonly ImportService _service;

    public ImportServiceTests()
    {
        _csvParserMock = new Mock<ICsvParser>();
        _publisherMock = new Mock<IRabbitMqPublisher>();

        _service = new ImportService(
            _csvParserMock.Object,
            _publisherMock.Object);
    }

    [Fact]
    public async Task ImportAsync_WhenBooksExist_PublishesAllBooks()
    {
        // Arrange
        var books = new List<BookImportMessage>
        {
            new()
            {
                Name = "Clean Code",
                Author = "Robert Martin",
                Genre = "Programming"
            },
            new()
            {
                Name = "The Hobbit",
                Author = "J.R.R. Tolkien",
                Genre = "Fantasy"
            }
        };

        _csvParserMock
            .Setup(x => x.Parse(It.IsAny<Stream>()))
            .Returns(books);

        using var stream = new MemoryStream();

        // Act
        var result = await _service.ImportAsync(stream);

        // Assert
        Assert.Equal(2, result);

        _publisherMock.Verify(
            x => x.PublishAsync(
                It.IsAny<BookImportMessage>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task ImportAsync_WhenNoValidRows_ThrowsInvalidDataException()
    {
        // Arrange
        _csvParserMock
            .Setup(x => x.Parse(It.IsAny<Stream>()))
            .Returns(new List<BookImportMessage>());

        using var stream = new MemoryStream();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidDataException>(() =>
            _service.ImportAsync(stream));

        _publisherMock.Verify(
            x => x.PublishAsync(
                It.IsAny<BookImportMessage>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}