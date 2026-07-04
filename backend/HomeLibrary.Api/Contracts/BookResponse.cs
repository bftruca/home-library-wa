namespace HomeLibrary.Api.Contracts
{
    public sealed record BookResponse(
    string Name,
    string Author,
    string Genre,
    DateTime ImportDate);
}
