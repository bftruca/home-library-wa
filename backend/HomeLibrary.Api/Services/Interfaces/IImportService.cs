namespace HomeLibrary.Api.Services.Interfaces
{
    public interface IImportService
    {
        Task<int> ImportAsync(
            Stream stream,
            CancellationToken cancellationToken = default);
    }
}
