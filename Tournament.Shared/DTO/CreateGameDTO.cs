using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Shared.DTO
{
    public class CreateGameDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int TournamentId { get; set; }
        [Required]
        public DateTime Time { get; set; }
        
    }
}
