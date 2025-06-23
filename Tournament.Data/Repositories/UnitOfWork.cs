using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TournamentContext _context;
        public ITournamentRepository TournamentRepository { get; }
        public IGameRepository GameRepository { get; }

        public UnitOfWork(TournamentContext context, 
            ITournamentRepository tournamentRepository, 
            IGameRepository gameRepository)

        {
            _context = context;
            TournamentRepository = tournamentRepository;
            GameRepository = gameRepository;
        }                

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
