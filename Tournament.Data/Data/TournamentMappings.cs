using AutoMapper;
using Domain.Models.Entities;
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
