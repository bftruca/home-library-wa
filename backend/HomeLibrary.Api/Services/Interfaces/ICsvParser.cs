using HomeLibrary.Shared.Messages;

namespace HomeLibrary.Api.Services.Interfaces
{
    public interface ICsvParser
    {
        IReadOnlyList<BookImportMessage> Parse(Stream stream);
    }
}
