using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;

namespace Tournament.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TournamentContext>();

            await db.Database.MigrateAsync();

            if (await db.TournamentDetails.AnyAsync())
            {
                // Seed initial data if the database is empty
                return;
            }

            
        }
    }
}
