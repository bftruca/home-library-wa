using HomeLibrary.Api.Contracts;
using HomeLibrary.Api.Services.Interfaces;
using HomeLibrary.Shared.RabbitMq.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client.Exceptions;

namespace HomeLibrary.Api.Controllers;

[ApiController]
[Route("api/imports")]
public sealed class ImportsController : ControllerBase
{
    private readonly IImportService _importService;

    public ImportsController(IImportService importService)
    {
        _importService = importService;
    }

    [HttpPost]
    [ProducesResponseType<ImportResponse>(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ImportAsync(
    IFormFile? file,
    CancellationToken cancellationToken)
    {
        if (file is null)
        {
            return BadRequest("No file was provided.");
        }

        if (file.Length == 0)
        {
            return BadRequest("The uploaded file is empty.");
        }

        if (!string.Equals(Path.GetExtension(file.FileName), ".csv", StringComparison.OrdinalIgnoreCase))
        {
            return StatusCode(
                StatusCodes.Status415UnsupportedMediaType,
                "Only CSV files are supported.");
        }

        using var stream = file.OpenReadStream();

        try
        {
            var queued = await _importService.ImportAsync(
                stream,
                cancellationToken);

            return Accepted(new ImportResponse(queued));
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (BrokerUnreachableException)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                "RabbitMQ is currently unavailable.");
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred while importing the file.");
        }
    }
}