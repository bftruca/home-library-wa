using HomeLibrary.Api.Services;
using HomeLibrary.Shared.Entities;
using HomeLibrary.Shared.Repositories.Interfaces;
using Moq;

namespace HomeLibrary.Tests;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _repositoryMock;
    private readonly BookService _service;

    public BookServiceTests()
    {
        _repositoryMock = new Mock<IBookRepository>();

        _service = new BookService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedBooks()
    {
        // Arrange
        var importDate = DateTime.UtcNow;

        _repositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Book>
            {
                new()
                {
                    Name = "Clean Code",
                    Author = "Robert Martin",
                    Genre = "Programming",
                    ImportDate = importDate
                },
                new()
                {
                    Name = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    Genre = "Fantasy",
                    ImportDate = importDate
                }
            });

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Collection(
            result,
            first =>
            {
                Assert.Equal("Clean Code", first.Name);
                Assert.Equal("Robert Martin", first.Author);
                Assert.Equal("Programming", first.Genre);
                Assert.Equal(importDate, first.ImportDate);
            },
            second =>
            {
                Assert.Equal("The Hobbit", second.Name);
                Assert.Equal("J.R.R. Tolkien", second.Author);
                Assert.Equal("Fantasy", second.Genre);
                Assert.Equal(importDate, second.ImportDate);
            });
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }
}