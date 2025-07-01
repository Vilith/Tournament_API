using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tournament.Shared.DTO;
using Tournament.Shared.Parameters;

namespace Services.Contracts
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDTO>> GetTournamentsAsync(TournamentFilterParameters parameters);
        Task<TournamentDTO?> GetTournamentAsync(int id);
    }
}
