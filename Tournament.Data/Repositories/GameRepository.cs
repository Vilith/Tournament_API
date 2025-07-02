using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;
using Domain.Contracts;
using Tournament.Shared.Parameters;
using Domain.Models.Entities;

namespace Tournament.Data.Repositories
{
    public class GameRepository(TournamentContext context) : IGameRepository
    {
        private readonly TournamentContext _context = context;


        public async Task<IEnumerable<Game>> GetFilteredAsync(GameFilterParameters parameters)
        {
            IQueryable<Game> query = _context.Games;                       

            if (!string.IsNullOrWhiteSpace(parameters.Title))
            {
                var lowerTitle = parameters.Title.ToLower();
                query = query.Where(g => g.Title != null && g.Title.ToLower().Contains(lowerTitle));
            }

            if (parameters.StartDate.HasValue)
            {
                query = query.Where(g => g.Time >= parameters.StartDate.Value);
            }

            if (parameters.EndDate.HasValue)
            {
                query = query.Where(g => g.Time <= parameters.EndDate.Value);
            }

            query = parameters.SortBy?.ToLower() switch
            {
                "title" => query.OrderBy(g => g.Title),
                "time" => query.OrderBy(g => g.Time),
                _ => query
            };

            query = query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetByTitleAsync(string title) => await _context.Games
            .Where(g => g.Title!.Contains(title))
            .ToListAsync();

        public async Task<IEnumerable<Game>> GetAllAsync() => await _context.Games
            .ToListAsync();

        public async Task<Game?> GetAsync(int id) => await _context.Games
            .FirstOrDefaultAsync(g => g.Id == id);

        public async Task<bool> AnyAsync(int id) => await _context.Games.AnyAsync(g => g.Id == id);       

        public void Add(Game game) => _context.Games.Add(game);
        
        public void Update(Game game) => _context.Games.Update(game);
    
        public void Remove(Game game) => _context.Games.Remove(game);      
    }
}
