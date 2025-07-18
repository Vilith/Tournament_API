using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Shared;
using Tournament.Shared.DTO;
using Tournament.Shared.Parameters;
using Tournament.Shared.Requests;

namespace Tournament.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GameDTO> CreateGameAsync(CreateGameDTO dto)
        {
            var entity = _mapper.Map<Game>(dto);

            if (!await _unitOfWork.TournamentRepository.AnyAsync(entity.TournamentId))
            {
                throw new NotFoundException($"Tournament with id: {entity.TournamentId} not found");
                //throw new InvalidOperationException($"Tournament with id: {entity.TournamentId} not found");
            }

            var tournamentGameCount = await _unitOfWork.GameRepository
                .CountAsync(g => g.TournamentId == entity.TournamentId);

            if (tournamentGameCount >= 10)
            {
                throw new BusinessRuleViolationException("A Tournament can not have more than 10 games");
                //throw new InvalidOperationException("A Tournament can not have more than 10 games");
            }

            if (await _unitOfWork.GameRepository.AnyAsync(g => g.Title == entity.Title && g.TournamentId == entity.TournamentId))
            {
                throw new InvalidOperationException("A Game with the same title already exists in this tournament");
            }

            _unitOfWork.GameRepository.Add(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<GameDTO>(entity);            
        }

        public async Task<bool> DeleteGameAsync(int id)
        {
            var entity = await _unitOfWork.GameRepository.GetAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.GameRepository.Remove(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<GameDTO?> GetGameAsync(int id)
        {
            Game? game = await _unitOfWork.GameRepository.GetAsync(id);
            return _mapper.Map<GameDTO>(game);
        }


        public async Task<(PagedList<GameDTO> Games, MetaData MetaData)> GetGamesAsync(GameFilterParameters parameters)
        {
            var filteredGames = await _unitOfWork.GameRepository.GetFilteredAsync(parameters);

            var mappedDtops = _mapper.Map<IEnumerable<GameDTO>>(filteredGames);

            var pagedList = PagedList<GameDTO>.ToPagedList(mappedDtops.AsQueryable(), parameters.PageNumber, parameters.PageSize);

            return (pagedList, pagedList.MetaData);

            //var filteredGames = await _unitOfWork.GameRepository.GetFilteredAsync(parameters);

            //int totalItems = filteredGames.Count();
            //int totalPages = (int)Math.Ceiling(totalItems / (double)parameters.PageSize);

            //var pagedGames = filteredGames
            //    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            //    .Take(parameters.PageSize)
            //    .ToList();

            ////var dto = _mapper.Map<IEnumerable<GameDTO>>(pagedGames);
            //var dto = _mapper.Map<List<GameDTO>>(pagedGames);
            //var pagedList = new PagedList<GameDTO>(dto, totalItems, parameters.PageNumber, parameters.PageSize);

            //var metaData = new MetaData
            //{
            //    TotalItems = totalItems,
            //    TotalPages = totalPages,
            //    PageSize = parameters.PageSize,
            //    CurrentPage = parameters.PageNumber
            //};

            //return (pagedList, metaData);
        }

        //public async Task<IEnumerable<GameDTO>> GetGamesAsync(GameFilterParameters parameters)
        //{
        //    return _mapper.Map<IEnumerable<GameDTO>>(await _unitOfWork.GameRepository.GetFilteredAsync(parameters));
        //}

        public async Task<IEnumerable<GameDTO>> GetGamesByTitleAsync(string title)
        {
            var games = await _unitOfWork.GameRepository.GetByTitleAsync(title);
            return _mapper.Map<IEnumerable<GameDTO>>(games);
        }

        public async Task<GameDTO?> PatchGameAsync(int id, JsonPatchDocument<GameDTO> patchDocument)
        {
            var entity = await _unitOfWork.GameRepository.GetAsync(id);
            if (entity == null)
                return null;

            var dto = _mapper.Map<GameDTO>(entity);

            patchDocument.ApplyTo(dto);            

            _mapper.Map(dto, entity);            

            await _unitOfWork.CompleteAsync();

            return dto;
        }

        public async Task<bool> UpdateGameAsync(int id, UpdateGameDTO dto)
        {
            if (!await _unitOfWork.GameRepository.AnyAsync(g => g.Id == id))
                return false;

            var entity = await _unitOfWork.GameRepository.GetAsync(id);
            if (entity == null)
                return false;

            entity.Title = dto.Title;
            entity.Time = dto.Time;

            _unitOfWork.GameRepository.Update(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }                
    }
}
