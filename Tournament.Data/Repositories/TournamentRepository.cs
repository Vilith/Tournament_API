using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;
using Tournament.Core.Entities;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class TournamentRepository(TournamentContext context) : ITournamentRepository
    {
        private readonly TournamentContext _context = context;


        public async Task<IEnumerable<TournamentDetails>> GetFilteredAsync(
            bool includeGames, 
            DateTime? startDate,
            DateTime? endDate,
            string? title,
            string? gameTitle,
            string? sortBy
            )
        {
            IQueryable<TournamentDetails> query = _context.TournamentDetails;

            if (includeGames)
            {
                query = query.Include(t => t.Games);
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                var lowerTitle = title.ToLower();
                query = query.Where(t => t.Title != null && t.Title.ToLower().Contains(lowerTitle));
            }

            if (startDate.HasValue)
            {
                query = query.Where(t => t.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.StartDate <= endDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(gameTitle) && includeGames)
            {
                var lowerGameTitle = gameTitle.ToLower();
                query = query.Where(t => t.Games!.Any(g => g.Title != null && g.Title.ToLower().Contains(lowerGameTitle)));
            }

            query = sortBy?.ToLower() switch
            {
                "title" => query.OrderBy(t => t.Title),
                "startdate" => query.OrderBy(t => t.StartDate),
                _ => query
            };

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TournamentDetails>> GetAllAsync(bool includeGames)
        {
            if (includeGames)
            {
                return await _context.TournamentDetails
                    .Include(t => t.Games)
                    .ToListAsync();
            }
            return await _context.TournamentDetails.ToListAsync();

        }

        public async Task<TournamentDetails?> GetAsync(int id, bool includeGames = false)
        {
            IQueryable<TournamentDetails> query = _context.TournamentDetails;
            if (includeGames)
                query = query.Include(t => t.Games);

            return await query.FirstOrDefaultAsync(t => t.Id == id);

            //return await _context.TournamentDetails
            //.Include(t => t.Games)
            //.FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<bool> AnyAsync(int id) => await _context.TournamentDetails.AnyAsync(t => t.Id == id);
        //{
            //return await _context.TournamentDetails.AnyAsync(t => t.Id == id);
        //}

        public void Add(TournamentDetails tournament) => _context.TournamentDetails.Add(tournament);
        //{
            //_context.TournamentDetails.Add(tournament);
        //}

        public void Update(TournamentDetails tournament) => _context.TournamentDetails.Update(tournament);
        //{
        //_context.TournamentDetails.Update(tournament);
        //}

        public void Remove(TournamentDetails tournament) => _context.TournamentDetails.Remove(tournament);
                
    }
}
