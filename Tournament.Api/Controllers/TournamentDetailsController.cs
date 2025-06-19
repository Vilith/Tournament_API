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
using Tournament.Core.DTO;
using AutoMapper;

namespace Tournament.Data.Controllers
{
    [Route("api/TournamentDetails")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentDetailsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDTO>>> GetTournamentDetails()
        {
            var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<TournamentDTO>>(tournaments);
            return Ok(dto);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDetails>> GetTournamentDetails(int id)
        {

            var dto = await _unitOfWork.TournamentRepository.GetAsync(id);

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TournamentDTO>(dto));

            //var tournamentDetails = await _unitOfWork.TournamentRepository.GetAsync(id);

            //if (tournamentDetails == null)
            //{
            //return NotFound();
            //}

            //return Ok(tournamentDetails);
        }

        // PUT: api/TournamentDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDTO dto)
        {
            if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
                return NotFound();

            var entity = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (entity == null)
                return NotFound();

            entity.Title = dto.Title;
            entity.StartDate = dto.StartDate;

            _unitOfWork.TournamentRepository.Update(entity);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        //if (id != tournamentDetails.Id)
        //{
        //return BadRequest();
        //}

        //if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
        //{
        //return NotFound();
        //}

        //_unitOfWork.TournamentRepository.Update(tournamentDetails);
        //await _unitOfWork.CompleteAsync();

        //return NoContent();
        //}

        //if (id != tournamentDetails.Id)
        //{
        //return BadRequest();
        //}

        //_context.Entry(tournamentDetails).State = EntityState.Modified;

        //try
        //{
        //await _context.SaveChangesAsync();
        //}
        //catch (DbUpdateConcurrencyException)
        //{
        //if (!TournamentDetailsExists(id))
        //{
        //return NotFound();
        //}
        //else
        //{
        //throw;
        //}
        //}

        //return NoContent();
        //}

        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDetails>> PostTournamentDetails(TournamentDTO dto)
        {
            var entity = _mapper.Map<TournamentDetails>(dto);
            _unitOfWork.TournamentRepository.Add(entity);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetTournamentDetails), new { id = entity.Id }, null);

            //_unitOfWork.TournamentRepository.Add(tournamentDetails);
            //await _unitOfWork.CompleteAsync();

            //return CreatedAtAction(nameof(GetTournamentDetails), new { id = tournamentDetails.Id }, tournamentDetails);

            //_context.TournamentDetails.Add(tournamentDetails);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTournamentDetails", new { id = tournamentDetails.Id }, tournamentDetails);
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var dto = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (dto == null)
            {
                return NotFound();
            }

            _unitOfWork.TournamentRepository.Remove(dto);
            await _unitOfWork.CompleteAsync();

            return NoContent();


            //var tournamentDetails = await _unitOfWork.TournamentRepository.GetAsync(id);
            //if (tournamentDetails == null)
            //{
            //return NotFound();
            //}

            //_unitOfWork.TournamentRepository.Remove(tournamentDetails);
            //await _unitOfWork.CompleteAsync();

            //return NoContent();
            //var tournamentDetails = await _context.TournamentDetails.FindAsync(id);
            //if (tournamentDetails == null)
            //{
            //    return NotFound();
            //}

            //_context.TournamentDetails.Remove(tournamentDetails);
            //await _context.SaveChangesAsync();

            //return NoContent();
        }

        private async Task<bool> TournamentDetailsExists(int id)
        {
            return await _unitOfWork.TournamentRepository.AnyAsync(id);
            //return _context.TournamentDetails.Any(e => e.Id == id);
        }
    }
}
