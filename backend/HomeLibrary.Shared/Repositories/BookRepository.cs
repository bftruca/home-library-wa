using HomeLibrary.Shared.Data;
using HomeLibrary.Shared.Entities;
using HomeLibrary.Shared.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeLibrary.Shared.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Book book, CancellationToken cancellationToken)
        {
            await _context.Books.AddAsync(book, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Books
            .OrderByDescending(x => x.ImportDate)
            .ToListAsync(cancellationToken);
        }
    }
}
