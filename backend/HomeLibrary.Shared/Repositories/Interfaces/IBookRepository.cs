using HomeLibrary.Shared.Entities;

namespace HomeLibrary.Shared.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task AddAsync(Book book, CancellationToken cancellationToken);

        Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken);
    }
}
