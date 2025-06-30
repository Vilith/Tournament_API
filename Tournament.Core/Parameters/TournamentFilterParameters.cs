using Microsoft.AspNetCore.Mvc;

namespace Tournament.Api.Parameters
{
    public class TournamentFilterParameters
    {        
        public bool IncludeGames { get; set; } = false;        
        public DateTime? StartDate { get; set; }        
        public DateTime? EndDate { get; set; }        
        public string? Title { get; set; }        
        public string? GameTitle { get; set; }        
        public string? SortBy { get; set; }

        public int PageNumber { get; set; } = 1;

        // Validation for PageSize to not exceed a maximum value
        private int _pageSize = 10;
        private readonly int maxPageSize = 50;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;

        }
    }
}
