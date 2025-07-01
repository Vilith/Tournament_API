using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tournament.Data.Data;

using Domain.Contracts;
using Domain.Models.Entities;
using Tournament.Shared.Parameters;

namespace Tournament.Data.Repositories
{
    public class TournamentRepository(TournamentContext context) : ITournamentRepository
    {
        private readonly TournamentContext _context = context;


        public async Task<IEnumerable<TournamentDetails>> GetFilteredAsync(TournamentFilterParameters parameters)
        {
            IQueryable<TournamentDetails> query = _context.TournamentDetails;

            if (parameters.IncludeGames)
            {
                query = query.Include(t => t.Games);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Title))
            {
                var lowerTitle = parameters.Title.ToLower();
                query = query.Where(t => t.Title != null && t.Title.ToLower().Contains(lowerTitle));
            }

            if (parameters.StartDate.HasValue)
            {
                query = query.Where(t => t.StartDate >= parameters.StartDate.Value);
            }

            if (parameters.EndDate.HasValue)
            {
                query = query.Where(t => t.StartDate <= parameters.EndDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(parameters.GameTitle) && parameters.IncludeGames)
            {
                var lowerGameTitle = parameters.GameTitle.ToLower();
                query = query.Where(t => t.Games!.Any(g => g.Title != null && g.Title.ToLower().Contains(lowerGameTitle)));
            }

            query = parameters.SortBy?.ToLower() switch
            {
                "title" => query.OrderBy(t => t.Title),
                "startdate" => query.OrderBy(t => t.StartDate),
                "enddate" => query.OrderBy(t => t.EndDate),
                _ => query
            };

            query = query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize);

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
        }

        public async Task<bool> AnyAsync(int id) => await _context.TournamentDetails.AnyAsync(t => t.Id == id);
       
        public void Add(TournamentDetails tournament) => _context.TournamentDetails.Add(tournament);
       
        public void Update(TournamentDetails tournament) => _context.TournamentDetails.Update(tournament);
       
        public void Remove(TournamentDetails tournament) => _context.TournamentDetails.Remove(tournament);
                
    }
}
