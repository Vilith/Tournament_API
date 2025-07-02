using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Tournament.Shared.DTO;
using Tournament.Shared.Parameters;
using Domain.Models.Entities;
using Domain.Contracts;
using Services.Contracts;


namespace Tournament.Presentation.Controllers
{
    [Route("api/TournamentDetails")]
    [ApiController]
    public class TournamentDetailsController(IServiceManager serviceManager) : ControllerBase
    {
        
        private readonly IServiceManager _serviceManager = serviceManager;

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDTO>>> GetTournamentDetails(
            [FromQuery] TournamentFilterParameters parameters)
        {
            var tournaments = await _serviceManager.TournamentService.GetTournamentDetailsAsync(parameters); 
            
            return Ok(tournaments);            
        }               

        // GET: api/TournamentDetails/5
               
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDTO>> GetTournamentDetails(int id)
        {
            var tournament = await _serviceManager.TournamentService.GetTournamentDetailsAsync(id);
            if (tournament == null)            
                return NotFound($"Tournament with ID {id} not found.");

            return Ok(tournament);

            //var dto = await _unitOfWork.TournamentRepository.GetAsync(id);

            //if (dto == null)
            //{
            //    return NotFound();
            //}

            //return Ok(_mapper.Map<TournamentDTO>(dto));                       
        }

        // PUT: api/TournamentDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, UpdateTournamentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _serviceManager.TournamentService.UpdateTournamentDetailsAsync(id, dto);
            if (!updated)
                return NotFound($"Tournament with ID {id} not found.");

            return NoContent();

            //if (!ModelState.IsValid)
            //{
            //return BadRequest(ModelState);
            //}

            //if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
            //return NotFound();

            //var entity = await _unitOfWork.TournamentRepository.GetAsync(id);
            //if (entity == null)
            //return NotFound();

            //_mapper.Map(dto, entity);

            //_unitOfWork.TournamentRepository.Update(entity);
            //await _unitOfWork.CompleteAsync();

            //return NoContent();
        }

        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDetails>> PostTournamentDetails(CreateTournamentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTournament = await _serviceManager.TournamentService.CreateTournamentDetailsAsync(dto);
            return CreatedAtAction(nameof(GetTournamentDetails), new { id = createdTournament.Id }, createdTournament);

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var entity = _mapper.Map<TournamentDetails>(dto);
            //_unitOfWork.TournamentRepository.Add(entity);
            //await _unitOfWork.CompleteAsync();

            //var createdDto = _mapper.Map<TournamentDTO>(entity);
            //return CreatedAtAction(nameof(GetTournamentDetails), new { id = entity.Id }, createdDto);                        
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var deleted = await _serviceManager.TournamentService.DeleteTournamentDetailsAsync(id);
            if (!deleted)            
                return NotFound($"Tournament with ID {id} not found.");

            return NoContent();

            //var dto = await _unitOfWork.TournamentRepository.GetAsync(id);
            //if (dto == null)
            //{
            //    return NotFound();
            //}

            //_unitOfWork.TournamentRepository.Remove(dto);
            //await _unitOfWork.CompleteAsync();

            //return NoContent();                                    
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<TournamentDTO>> PatchTournament(int id, JsonPatchDocument<TournamentDTO> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest("Patch document cannot be null.");

            var patchedTournament = await _serviceManager.TournamentService.PatchTournamentDetailsAsync(id, patchDocument);
            if (patchedTournament == null)            
                return NotFound($"Tournament with ID {id} not found.");

            if (!ModelState.IsValid)            
                return UnprocessableEntity(ModelState);

            return Ok(patchedTournament);

            //if (patchDocument == null)            
            //    return BadRequest("Patch document cannot be null.");

            //var tournamentExists = await _unitOfWork.TournamentRepository.GetAsync(id);
            //if (tournamentExists == null) 
            //    return NotFound($"Tournament with ID {id} not found.");

            //var dto = _mapper.Map<TournamentDTO>(tournamentExists);
            //patchDocument.ApplyTo(dto, ModelState);
            //TryValidateModel(dto);

            //if (!ModelState.IsValid)            
            //    return UnprocessableEntity(ModelState);

            //_mapper.Map(dto, tournamentExists);            
            //await _unitOfWork.CompleteAsync();

            ////return Ok(dto);
            //return dto;
        }              
    }
}
