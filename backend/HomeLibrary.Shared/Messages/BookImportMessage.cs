using HomeLibrary.Shared.Entities;

namespace HomeLibrary.Shared.Messages
{
    public sealed record BookImportMessage
    {
        public required string Name { get; init; }

        public required string Author { get; init; }

        public required string Genre { get; init; }

        public Book ToEntity()
        {
            return new Book
            {
                Name = Name,
                Author = Author,
                Genre = Genre,
                ImportDate = DateTime.UtcNow
            };
        }
    }
}
