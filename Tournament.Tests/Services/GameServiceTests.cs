using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tournament.Services;
using Tournament.Shared.DTO;

namespace Tournament.Tests.Services
{
    public class GameServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGameRepository> _mockGameRepo;
        private readonly Mock<ITournamentRepository> _mockTournamentRepo;
        private readonly Mock<IMapper> _mockMapper;

        private readonly GameService _gameService;

        public GameServiceTests() 
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockGameRepo = new Mock<IGameRepository>();
            _mockTournamentRepo = new Mock<ITournamentRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.SetupGet(u => u.GameRepository).Returns(_mockGameRepo.Object);
            _mockUnitOfWork.SetupGet(u => u.TournamentRepository).Returns(_mockTournamentRepo.Object);

            _gameService = new GameService(_mockUnitOfWork.Object, _mockMapper.Object);

        }

        [Fact]
        public async Task CreateGameAsync_ShouldAddGame_WhenValid()
        {
            // Arrange
            var dto = new CreateGameDTO { Title = "Game", TournamentId = 1 };
            var gameEntity = new Game { Id = 1, Title = dto.Title, TournamentId = dto.TournamentId };

            _mockTournamentRepo.Setup(r => r.AnyAsync(1)).ReturnsAsync(true);
            _mockGameRepo.Setup(r => r.CountAsync(It.IsAny<Expression<Func<Game, bool>>>())).ReturnsAsync(5);
            _mockGameRepo.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Game, bool>>>())).ReturnsAsync(false);

            _mockMapper.Setup(m => m.Map<Game>(dto)).Returns(gameEntity);
            _mockMapper.Setup(m => m.Map<GameDTO>(gameEntity)).Returns(new GameDTO { Id = 1, Title = dto.Title });

            // Act
            var result = await _gameService.CreateGameAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(dto.Title, result.Title);

            _mockGameRepo.Verify(r => r.Add(It.Is<Game>(g => g.Title == dto.Title && g.TournamentId == dto.TournamentId)), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateGameAsync_ShouldThrow_WhenTournamentHas10Games()
        {
            // Arrange
            var dto = new CreateGameDTO { Title = "Game", TournamentId = 1 };

            _mockTournamentRepo.Setup(r => r.AnyAsync(1)).ReturnsAsync(true);

            _mockGameRepo.Setup(r => r.CountAsync(It.IsAny<Expression<Func<Game, bool>>>())).ReturnsAsync(10);

            _mockMapper.Setup(m => m.Map<Game>(It.IsAny<CreateGameDTO>())).Returns(new Game { Id = 1, Title = dto.Title, TournamentId = dto.TournamentId });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _gameService.CreateGameAsync(dto));

            Assert.Equal("A Tournament can not have more than 10 games", exception.Message);

            _mockGameRepo.Verify(r => r.Add(It.IsAny<Game>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }

        //[Fact]
        //public async Task CreateGameAsync_Should_ThrowException_When_Game_Title_Already_Exists_In_Tournament()
        //{
            
        //}
    }
}
