using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace Tournament.Data.Data
{
    public class TournamentContext(DbContextOptions<TournamentContext> options) : DbContext(options)
    {
        public DbSet<Domain.Models.Entities.TournamentDetails> TournamentDetails { get; set; } = default!;
        public DbSet<Domain.Models.Entities.Game> Games { get; set; } = default!;
    }
}
