using HomeLibrary.Api.Contracts;

namespace HomeLibrary.Api.Services.Interfaces
{
    public interface IBookService
    {
        Task<IReadOnlyList<BookResponse>> GetAllAsync(
            CancellationToken cancellationToken = default);
    }
}
