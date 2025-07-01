using AutoMapper;
using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Shared.DTO;


namespace Tournament.Data.Data
{
    public class TournamentMappings : Profile
    {
        public TournamentMappings() 
        {            
            CreateMap<Game, GameDTO>().ReverseMap();
            CreateMap<UpdateGameDTO, Game>().ReverseMap();
            CreateMap<CreateGameDTO, Game>();

            CreateMap<TournamentDetails, TournamentDTO>().ReverseMap();
            CreateMap<TournamentDetails, UpdateTournamentDTO>().ReverseMap();
            CreateMap<CreateTournamentDTO, TournamentDetails>();           
                
        }
    }
}
