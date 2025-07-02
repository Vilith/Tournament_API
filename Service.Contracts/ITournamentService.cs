using Microsoft.AspNetCore.JsonPatch;
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
        Task<IEnumerable<TournamentDTO>> GetTournamentDetailsAsync(TournamentFilterParameters parameters);
        Task<TournamentDTO?> GetTournamentDetailsAsync(int id);
        Task<TournamentDTO> CreateTournamentDetailsAsync(CreateTournamentDTO dto);
        Task<TournamentDTO?> PatchTournamentDetailsAsync(int id, JsonPatchDocument<TournamentDTO> patchDocument);
        Task<bool> DeleteTournamentDetailsAsync(int id);
        Task<bool> UpdateTournamentDetailsAsync(int id, UpdateTournamentDTO dto);

    }
}
