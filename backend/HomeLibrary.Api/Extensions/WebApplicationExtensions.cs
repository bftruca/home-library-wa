using HomeLibrary.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeLibrary.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<LibraryDbContext>();

        dbContext.Database.Migrate();

        return app;
    }
}