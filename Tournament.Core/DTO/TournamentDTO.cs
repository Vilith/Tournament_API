using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.DTO
{
    public class TournamentDTO
    {        
        public string? Title { get; set; }
        public DateTime StartDate { get; set; }
    }
}
