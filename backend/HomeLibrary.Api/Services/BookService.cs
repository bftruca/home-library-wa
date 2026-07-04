using HomeLibrary.Api.Contracts;
using HomeLibrary.Api.Services.Interfaces;
using HomeLibrary.Shared.Repositories.Interfaces;

namespace HomeLibrary.Api.Services
{
    public sealed class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IReadOnlyList<BookResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var books = await _bookRepository.GetAllAsync(cancellationToken);

            return books
                .Select(book => new BookResponse(
                    book.Name,
                    book.Author,
                    book.Genre,
                    book.ImportDate))
                .ToList();
        }
    }
}
