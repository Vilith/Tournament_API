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
    public interface IGameService
    {
        Task<(PagedList<GameDTO> Games, MetaData MetaData)> GetGamesAsync(GameFilterParameters parameters);
        Task<IEnumerable<GameDTO>> GetGamesByTitleAsync(string title);
        Task<GameDTO?> GetGameAsync(int id);
        Task<GameDTO> CreateGameAsync(CreateGameDTO dto);
        Task<GameDTO?> PatchGameAsync(int id, JsonPatchDocument<GameDTO> patchDocument);
        Task<bool> DeleteGameAsync(int id);
        Task<bool> UpdateGameAsync(int id, UpdateGameDTO dto);
    }
}