
using Tournament.Shared.Requests;

namespace Tournament.Shared.Parameters
{
    public class GameFilterParameters : RequestParameters
    {
        public DateTime? StartDate { get; set; }     
        public DateTime? EndDate { get; set; }        
        public string? Title { get; set; }        
        public string? SortBy { get; set; }
        
    }
}
