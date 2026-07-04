using HomeLibrary.Api.Contracts;
using HomeLibrary.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeLibrary.Api.Controllers;

[ApiController]
[Route("api/books")]
public sealed class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<BookResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<BookResponse>>> GetAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var books = await _bookService.GetAllAsync(cancellationToken);

            return Ok(books);
        }
        catch (Npgsql.NpgsqlException)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                "Database is currently unavailable.");
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.");
        }
    }
}