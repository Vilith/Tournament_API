using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.DTO
{
    public class GameDTO
    {
        public int Id { get; init; }
        public string? Title { get; init; }
        public DateTime Time { get; init; }        
    }
}
