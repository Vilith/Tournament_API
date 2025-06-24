using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
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

            // Example seed data
            var tournaments = new List<TournamentDetails>
            {
                new TournamentDetails
                {
                    Title = "Katowice Championship",
                    StartDate = DateTime.Now.AddDays(30),
                    Games = new List<Game>
                    {
                        new Game { Title = "Minesweeper", Time = DateTime.Now.AddDays(31) },
                        new Game { Title = "Solitaire", Time = DateTime.Now.AddDays(33) }
                    }
                },

                new TournamentDetails
                {
                    Title = "SLIEL - Sex Laxar I En Laxask",
                    StartDate = DateTime.Now.AddDays(60),
                    Games = new List<Game>
                    {
                        new Game { Title = "Chess", Time = DateTime.Now.AddDays(61) },
                        new Game { Title = "Checkers", Time = DateTime.Now.AddDays(63) }
                    }
            }
            };

            db.TournamentDetails.AddRange(tournaments);
            await db.SaveChangesAsync();
        }
    }
}
