using Bogus;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public static class SeedData
    {
        public static async Task InitAsync(TournamentContext context)
        {
            if (await context.TournamentDetails.AnyAsync())
            {
                return; // Database already seeded
            }

            var gameTitle = new[]
            {
                "League of Legends", "Dota 2", "Counter-Strike: Global Offensive",
                "Valorant", "Overwatch", "Call of Duty", "FIFA", "PUBG", "Apex Legends", "Rainbow Six Siege",
                "StarCraft II", "Hearthstone", "Fortnite", "Rocket League", "World of Warcraft"
            };

            var gameFaker = new Faker<Game>()
                .RuleFor(g => g.Title, f => f.PickRandom(gameTitle))
                .RuleFor(g => g.Time, f => f.Date.Future());

            var tournamentFaker = new Faker<TournamentDetails>()
                .RuleFor(t => t.Title, f => $"{f.Company.CompanyName()} Championship")
                .RuleFor(t => t.StartDate, f => f.Date.Soon(30))
                .RuleFor(t => t.Games, f => gameFaker.Generate(f.Random.Int(2, 5)));

            var tournaments = tournamentFaker.Generate(3);

            context.TournamentDetails.AddRange(tournaments);
            await context.SaveChangesAsync();

        }
    }
}
