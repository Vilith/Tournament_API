using Domain.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Tournament.Shared.DTO;
using Tournament.Shared.Parameters;

namespace Tournament.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController(IServiceManager serviceManager) : ControllerBase
    {
        private readonly IServiceManager _serviceManager = serviceManager;

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDTO>>> GetGames(
            [FromQuery] GameFilterParameters parameters)
        {
            var (games, metaData) = await _serviceManager.GameService.GetGamesAsync(parameters);

            var response = new
            {
                items = games,
                metaData = new
                {
                    totalPages = metaData.TotalPages,
                    pageSize = metaData.PageSize,
                    currentPage = metaData.CurrentPage,
                    totalItems = metaData.TotalItems
                }
            };

            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GameDTO>>> GetGamesByTitle(string title)
        {
            var games = await _serviceManager.GameService.GetGamesByTitleAsync(title);
            if (!games.Any())
                return NotFound($"No games with title: {title} found!");

            return Ok(games);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _serviceManager.GameService.GetGameAsync(id);
            if (game == null)
                return NotFound($"Game with ID {id} not found.");

            return Ok(game);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, UpdateGameDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateResult = await _serviceManager.GameService.UpdateGameAsync(id, dto);
            if (!updateResult)
                return NotFound($"Game with ID {id} not found or update failed.");

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(CreateGameDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _serviceManager.GameService.CreateGameAsync(dto);
            return CreatedAtAction(nameof(GetGame), new { id = created.Id }, created);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var deleted = await _serviceManager.GameService.DeleteGameAsync(id);
            if (!deleted)
                return NotFound($"Game with ID {id} not found.");

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<GameDTO>> PatchGame(int id, JsonPatchDocument<GameDTO> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest("Patch document cannot be null.");

            var patchedGame = await _serviceManager.GameService.PatchGameAsync(id, patchDocument);
            if (patchedGame == null)
                return NotFound($"Game with ID {id} not found.");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            return Ok(patchedGame);
        }
    }
}
