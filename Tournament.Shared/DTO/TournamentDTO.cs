﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Shared.DTO
{
    public class TournamentDTO
    {
        public int Id { get; init; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(60, ErrorMessage = "Name of the title has to be less than 60 characters")]
        public string? Title { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }

        public List<GameDTO>? Games { get; init; }
        
    }
}
