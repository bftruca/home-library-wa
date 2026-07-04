namespace HomeLibrary.Shared.Messages
{
    public sealed record BookImportMessage
    {
        public required string Name { get; init; }

        public required string Author { get; init; }

        public required string Genre { get; init; }
    }
}
