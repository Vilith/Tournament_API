using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Shared.DTO;
using Tournament.Shared.Parameters;

namespace Tournament.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TournamentDTO> CreateTournamentDetailsAsync(CreateTournamentDTO dto)
        {
            var entity = _mapper.Map<TournamentDetails>(dto);
            _unitOfWork.TournamentRepository.Add(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<TournamentDTO>(entity);
        }

        public async Task<bool> DeleteTournamentDetailsAsync(int id)
        {
            var entity = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.TournamentRepository.Remove(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }               

        public async Task<IEnumerable<TournamentDTO>> GetTournamentDetailsAsync(TournamentFilterParameters parameters)
        {
            return _mapper.Map<IEnumerable<TournamentDTO>>(await _unitOfWork.TournamentRepository.GetFilteredAsync(parameters));
        }

        public async Task<TournamentDTO?> GetTournamentDetailsAsync(int id)
        {            
            TournamentDetails? tournament = await _unitOfWork.TournamentRepository.GetAsync(id, true);
            if (tournament == null)
                return null;

            return _mapper.Map<TournamentDTO>(tournament);
        }

        public async Task<TournamentDTO?> PatchTournamentDetailsAsync(int id, JsonPatchDocument<TournamentDTO> patchDocument)
        {
            var entity = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (entity == null)
                return null;

            var dto = _mapper.Map<TournamentDTO>(entity);

            patchDocument.ApplyTo(dto);

            _mapper.Map(dto, entity);

            await _unitOfWork.CompleteAsync();

            return dto;
        }

        public async Task<bool> UpdateTournamentDetailsAsync(int id, UpdateTournamentDTO dto)
        {
            if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
                return false;

            var entity = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (entity == null)
                return false;
                                    
            _mapper.Map(dto, entity);

            _unitOfWork.TournamentRepository.Update(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
