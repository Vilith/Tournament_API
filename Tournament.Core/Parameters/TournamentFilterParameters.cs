using Microsoft.AspNetCore.Mvc;

namespace Tournament.Api.Parameters
{
    public class TournamentFilterParameters
    {
        [FromQuery]
        public bool IncludeGames { get; set; } = false;

        [FromQuery]
        public DateTime? StartDate { get; set; }

        [FromQuery]
        public DateTime? EndDate { get; set; }

        [FromQuery]
        public string? Title { get; set; }

        [FromQuery]
        public string? GameTitle { get; set; }

        [FromQuery]
        public string? SortBy { get; set; }
    }
}
