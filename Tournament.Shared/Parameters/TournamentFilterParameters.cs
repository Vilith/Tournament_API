
using Tournament.Shared.Requests;

namespace Tournament.Shared.Parameters
{
    public class TournamentFilterParameters : RequestParameters
    {        
        public bool IncludeGames { get; set; } = false;        
        public DateTime? StartDate { get; set; }        
        public DateTime? EndDate { get; set; }        
        public string? Title { get; set; }        
        public string? GameTitle { get; set; }        
        public string? SortBy { get; set; }
        
    }
}
