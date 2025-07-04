using AutoMapper;
using Domain.Contracts;
using Services.Contracts;

namespace Tournament.Services
{
    public class ServiceManager : IServiceManager
    {
        //private readonly IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;

        //private ITournamentService _tournamentService;
        //private IGameService _gameService;

        //public ITournamentService TournamentService => _tournamentService ??= new TournamentService(_unitOfWork, _mapper);
        //public IGameService GameService => _gameService ??= new GameService(_unitOfWork, _mapper);

        //public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
        //{
        //    _unitOfWork = unitOfWork;
        //    _mapper = mapper;
        //}                

        // From my understanding - The snippet above is more or less the same as the snippet below.
        // The major drawback from the upper snippet is it's threading security.
        // (Something may behave in an unwanted way if many calls are made at the same time)

        private readonly Lazy<ITournamentService> _tournamentService;
        private readonly Lazy<IGameService> _gameService;

        public ITournamentService TournamentService => _tournamentService.Value;
        public IGameService GameService => _gameService.Value;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _tournamentService = new Lazy<ITournamentService>(() => new TournamentService(unitOfWork, mapper));
            _gameService = new Lazy<IGameService>(() => new GameService(unitOfWork, mapper));
        }
    }
}
