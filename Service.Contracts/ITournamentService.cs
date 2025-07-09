using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Shared;
using Tournament.Shared.DTO;
using Tournament.Shared.Parameters;
using Tournament.Shared.Requests;

namespace Services.Contracts
{
    public interface ITournamentService
    {        
        Task<(PagedList<TournamentDTO> Tournaments, MetaData MetaData)> GetTournamentDetailsAsync(TournamentFilterParameters parameters);
        Task<TournamentDTO?> GetTournamentDetailsAsync(int id);
        Task<TournamentDTO> CreateTournamentDetailsAsync(CreateTournamentDTO dto);
        Task<TournamentDTO?> PatchTournamentDetailsAsync(int id, JsonPatchDocument<TournamentDTO> patchDocument);
        Task<bool> DeleteTournamentDetailsAsync(int id);
        Task<bool> UpdateTournamentDetailsAsync(int id, UpdateTournamentDTO dto);

    }
}
