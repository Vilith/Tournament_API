using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using AutoMapper;
using Tournament.Core.DTO;

namespace Tournament.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GamesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<Game>>(games);
            return Ok(dto);
            //var game = await _unitOfWork.GameRepository.GetAllAsync();
            //return Ok(game);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var dto = await _unitOfWork.GameRepository.GetAsync(id);

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GameDTO>(dto));

            //var game = await _unitOfWork.GameRepository.GetAsync(id);
            ////var game = await _context.Games.FindAsync(id);

            //if (game == null)
            //{
                //return NotFound();
            //}

            //return Ok(game);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDTO dto)
        {
            if (!await _unitOfWork.GameRepository.AnyAsync(id))            
                return NotFound();
            
            var entity = await _unitOfWork.GameRepository.GetAsync(id);
            if (entity == null)            
                return NotFound();

            entity.Title = dto.Title;
            entity.Time = dto.Time;

            _unitOfWork.GameRepository.Update(entity);
            await _unitOfWork.CompleteAsync();

            return NoContent();


            //if (id != game.Id)
            //{
                //return BadRequest();
            //}

            //if (!await _unitOfWork.GameRepository.AnyAsync(id))
            //{
                //return NotFound();
            //}

            //_unitOfWork.GameRepository.Update(game);
            //await _unitOfWork.CompleteAsync();

            //return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(GameDTO dto)
        {
            var entity = _mapper.Map<Game>(dto);
            _unitOfWork.GameRepository.Add(entity);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetGame), new { id = entity.Id }, null);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var dto = await _unitOfWork.GameRepository.GetAsync(id);
            if (dto == null)
            {
                return NotFound();
            }

            _unitOfWork.GameRepository.Remove(dto);
            await _unitOfWork.CompleteAsync();

            return NoContent();

        }
            //var game = await _context.Games.FindAsync(id);
            //if (game == null)
            //{
            //    return NotFound();
            //}

            //_context.Games.Remove(game);
            //await _context.SaveChangesAsync();

            //return NoContent();
        

        private async Task<bool> GameExists(int id)
        {
            return await _unitOfWork.GameRepository.AnyAsync(id);
            //return _context.Games.Any(e => e.Id == id);
        }
    }
}
