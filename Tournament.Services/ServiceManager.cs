using AutoMapper;
using Domain.Contracts;
using Services.Contracts;

namespace Tournament.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private ITournamentService _tournamentService;
        private IGameService _gameService;

        public ITournamentService TournamentService => _tournamentService ??= new TournamentService(_unitOfWork, _mapper);
        public IGameService GameService => _gameService ??= new GameService(_unitOfWork, _mapper);

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }                
    }
}
