using Bogus;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;


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

            var tournaments = new List<TournamentDetails>();

            var faker = new Faker();
            int nrOfTournaments = faker.Random.Int(10, 25);

            for (int i = 0; i < nrOfTournaments; i++)
            {
                tournaments.Add(GenerateTournamentWithGames());
            }

            context.TournamentDetails.AddRange(tournaments);
            await context.SaveChangesAsync();
        }

        private static TournamentDetails GenerateTournamentWithGames()
        {

            var gameTitles = new[]
            {
                "League of Legends", "Dota 2", "Counter-Strike: Global Offensive",
                "Valorant", "Overwatch", "Call of Duty", "FIFA", "PUBG", "Apex Legends", "Rainbow Six Siege",
                "StarCraft II", "Hearthstone", "Fortnite", "Rocket League", "World of Warcraft"
            };

            var gameFaker = new Faker();

            var startDate = gameFaker.Date.Soon(15);
            var endDate = startDate.AddDays(gameFaker.Random.Int(1, 7));

            var tournament = new TournamentDetails
            {
                Title = $"{gameFaker.Company.CompanyName()} Championship",
                StartDate = startDate,
                EndDate = endDate,
                Games = GenerateNonOverlappingGames(startDate, endDate, gameTitles)
            };

            return tournament;
        }

        private static List<Game> GenerateNonOverlappingGames(DateTime startDate, DateTime endDate, string[] gameTitles)
        {
            var games = new List<Game>();
            var faker = new Faker();
            int numberOfGames = faker.Random.Int(5, 15);

            var gameDates = GenerateNonOverlappingDates(startDate, endDate, numberOfGames);

            for (int i = 0; i < numberOfGames; i++)
            {
                var game = (new Game
                {
                    Title = faker.PickRandom(gameTitles),
                    Time = gameDates[i]
                });

                games.Add(game);
            }

            return games;
        }

        private static List<DateTime> GenerateNonOverlappingDates(DateTime startDate, DateTime endDate, int count)
        {
            var faker = new Faker();
            var dates = new HashSet<DateTime>();

            while (dates.Count < count)
            {
                var randomDate = faker.Date.Between(startDate, endDate).Date.AddHours(faker.Random.Int(10, 20));
                dates.Add(randomDate);
            }

            return dates.OrderBy(d => d).ToList();
        }
    }
}

