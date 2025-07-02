using AutoMapper;
using Domain.Contracts;
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
        //private readonly IUnitOfWork _unitOfWork = unitOfWork;
        //private readonly IMapper _mapper = mapper;
        private readonly IServiceManager _serviceManager = serviceManager;

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDTO>>> GetGame(
            [FromQuery] GameFilterParameters parameters)
        {
            var games = await _serviceManager.GameService.GetGamesAsync(parameters);
            //var games = await _unitOfWork.GameRepository.GetFilteredAsync(parameters);
            //var dto = _mapper.Map<IEnumerable<GameDTO>>(games);
            return Ok(games);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GameDTO>>> GetGamesByTitle(string title)
        {
            var games = await _serviceManager.GameService.GetGamesByTitleAsync(title);
            if (!games.Any())            
                return NotFound($"No games with title: {title} found!");

            return Ok(games);

            //var games = await _unitOfWork.GameRepository.GetByTitleAsync(title);
            //if (!games.Any())            
            //return NotFound($"No games with title: {title} found!");

            //var dto = _mapper.Map<IEnumerable<GameDTO>>(games);
            //return Ok(dto);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _serviceManager.GameService.GetGameAsync(id);
            if (game == null)            
                return NotFound($"Game with ID {id} not found.");

            return Ok(game);

            //var dto = await _unitOfWork.GameRepository.GetAsync(id);

            //if (dto == null)            
            //    return NotFound($"Game with ID {id} not found.");


            //return Ok(_mapper.Map<GameDTO>(dto));
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

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (!await _unitOfWork.GameRepository.AnyAsync(id))            
            //    return NotFound($"Game with ID {id} not found.");

            //var entity = await _unitOfWork.GameRepository.GetAsync(id);
            //if (entity == null)            
            //    return NotFound($"Game with ID {id} not found.");

            //entity.Title = dto.Title;
            //entity.Time = dto.Time;

            //_unitOfWork.GameRepository.Update(entity);
            //await _unitOfWork.CompleteAsync();

            //return NoContent();
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

            //// Validate the DTO before mapping
            //var entity = _mapper.Map<Game>(dto);
            //_unitOfWork.GameRepository.Add(entity);
            //await _unitOfWork.CompleteAsync();

            //// Mapping the entity back to DTO for the response
            //var createdDto = _mapper.Map<GameDTO>(entity);
            //return CreatedAtAction(nameof(GetGame), new { id = entity.Id }, createdDto);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var deleted = await _serviceManager.GameService.DeleteGameAsync(id);
            if (!deleted)
                return NotFound($"Game with ID {id} not found.");

            return NoContent();

            //var dto = await _unitOfWork.GameRepository.GetAsync(id);
            //if (dto == null)
            //{
            //    return NotFound($"Couldn't delete. Game with ID {id} not found.");
            //}

            //_unitOfWork.GameRepository.Remove(dto);
            //await _unitOfWork.CompleteAsync();

            //return NoContent();
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

            //if (patchDocument == null)
            //    return BadRequest("Patch document cannot be null.");

            //var existingGame = await _unitOfWork.GameRepository.GetAsync(id);
            //if (existingGame == null)
            //    return NotFound($"Game with ID {id} not found.");

            //var dto = _mapper.Map<GameDTO>(existingGame);
            //patchDocument.ApplyTo(dto, ModelState);
            //TryValidateModel(dto);

            //if (!ModelState.IsValid)
            //    return UnprocessableEntity(ModelState);

            //_mapper.Map(dto, existingGame);            
            //await _unitOfWork.CompleteAsync();

            //return Ok(dto);
        }
    }
}
