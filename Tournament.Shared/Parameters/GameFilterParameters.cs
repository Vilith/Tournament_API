﻿
namespace Tournament.Shared.Parameters
{
    public class GameFilterParameters
    {
        public DateTime? StartDate { get; set; }     
        public DateTime? EndDate { get; set; }        
        public string? Title { get; set; }        
        public string? SortBy { get; set; }

        public int PageNumber { get; set; } = 1;

        // Validation for PageSize to not exceed a maximum value
        private int _pageSize = 20;
        private readonly int maxPageSize = 100;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;

        }
    }
}
