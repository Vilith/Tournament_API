using Domain.Contracts;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class UnitOfWork(
        TournamentContext context,
        ITournamentRepository tournamentRepository,
        IGameRepository gameRepository) : IUnitOfWork
    {
        private readonly TournamentContext _context = context;
        public ITournamentRepository TournamentRepository { get; } = tournamentRepository;
        public IGameRepository GameRepository { get; } = gameRepository;

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
