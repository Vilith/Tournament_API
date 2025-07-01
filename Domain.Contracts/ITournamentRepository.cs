using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models.Entities;
using Tournament.Shared.Parameters;

namespace Domain.Contracts
{

    public interface ITournamentRepository
    {
        //Task<IEnumerable<TournamentDetails>> GetFilteredAsync(bool includeGames, DateTime? startDate, DateTime? endDate, string? title, string? gameTitle, string? sortBy);
        Task<IEnumerable<TournamentDetails>> GetFilteredAsync(TournamentFilterParameters parameters);
        Task<IEnumerable<TournamentDetails>> GetAllAsync(bool includeGames);
        Task<TournamentDetails?> GetAsync(int id, bool includeGames = false);
        Task<bool> AnyAsync(int id);
        void Add(TournamentDetails tournament);
        void Update(TournamentDetails tournament);
        void Remove(TournamentDetails tournament);
    }
}