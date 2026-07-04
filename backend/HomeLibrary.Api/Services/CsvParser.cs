using HomeLibrary.Api.Services.Interfaces;
using HomeLibrary.Shared.Messages;
using System.Text;

namespace HomeLibrary.Api.Services
{
    public sealed class CsvParser : ICsvParser
    {
        public IReadOnlyList<BookImportMessage> Parse(Stream stream)
        {
            using var reader = new StreamReader(stream);

            var header = reader.ReadLine();

            if (header is null)
            {
                throw new InvalidDataException("CSV is empty.");
            }

            if (!string.Equals(header.Trim(), "name,author,genre", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException("Invalid CSV header.");
            }

            var books = new List<BookImportMessage>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var columns = line.Split(',');

                if (columns.Length != 3)
                {
                    continue;
                }

                books.Add(new BookImportMessage
                {
                    Name = columns[0].Trim(),
                    Author = columns[1].Trim(),
                    Genre = columns[2].Trim()
                });
            }

            return books;
        }
    }
}
