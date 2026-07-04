using System.Text;
using HomeLibrary.Api.Services;

namespace HomeLibrary.Tests;

public class CsvParserTests
{
    private readonly CsvParser _parser = new();

    [Fact]
    public void Parse_ValidCsv_ReturnsBooks()
    {
        // Arrange
        const string csv =
            """
            name,author,genre
            Clean Code,Robert Martin,Programming
            The Hobbit,J.R.R. Tolkien,Fantasy
            """;

        using var stream = CreateStream(csv);

        // Act
        var result = _parser.Parse(stream);

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Collection(
            result,
            first =>
            {
                Assert.Equal("Clean Code", first.Name);
                Assert.Equal("Robert Martin", first.Author);
                Assert.Equal("Programming", first.Genre);
            },
            second =>
            {
                Assert.Equal("The Hobbit", second.Name);
                Assert.Equal("J.R.R. Tolkien", second.Author);
                Assert.Equal("Fantasy", second.Genre);
            });
    }

    [Fact]
    public void Parse_EmptyCsv_ThrowsInvalidDataException()
    {
        // Arrange
        using var stream = CreateStream(string.Empty);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _parser.Parse(stream));
    }

    [Fact]
    public void Parse_InvalidHeader_ThrowsInvalidDataException()
    {
        // Arrange
        const string csv =
            """
            title,writer,type
            Clean Code,Robert Martin,Programming
            """;

        using var stream = CreateStream(csv);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _parser.Parse(stream));
    }

    [Fact]
    public void Parse_InvalidRows_SkipsRowsWithInvalidColumnCount()
    {
        // Arrange
        const string csv =
            """
            name,author,genre
            Book 1,Author 1,Fantasy
            Invalid Row
            Book 2,Author 2,Sci-Fi
            """;

        using var stream = CreateStream(csv);

        // Act
        var result = _parser.Parse(stream);

        // Assert
        Assert.Equal(2, result.Count);
    }

    private static MemoryStream CreateStream(string content)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(content));
    }
}