namespace HomeLibrary.Api.Contracts
{
    public sealed record BookResponse(
        int id,
        string Name,
        string Author,
        string Genre,
        DateTime ImportDate);
}
