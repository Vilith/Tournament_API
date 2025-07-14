using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Serialization;
using Services.Contracts;
using System.Linq.Expressions;
using Tournament.Presentation.Controllers;
using Tournament.Services;
using Tournament.Shared.DTO;
using Tournament.Shared.Parameters;
using Tournament.Tests.TestHelpers;

namespace Tournament.Tests.Controllers
{
    public class GamesControllerTests : ControllerTestBase<GamesController>
    {
        private readonly Mock<IGameService> MockGameService = new();
        public GamesControllerTests() : base((serviceManager) => new GamesController(serviceManager))
        {
            MockService.Setup(sm => sm.GameService).Returns(MockGameService.Object);
        }

        [Trait("GamesController", "Get")]
        [Fact]
        public async Task GetGames_ReturnsOk_WithGamesList()
        {
            // Arrange
            var gamesDto = new List<GameDTO>
            {
                new() { Id = 1, Title = "Game 1" },
                new() { Id = 2, Title = "Game 2" }
            };

            //MockGameService
            //    .Setup(s => s.GetGamesAsync(It.IsAny<GameFilterParameters>()))
            //    .ReturnsAsync(gamesDto);
            
            //MockMapper.Setup(m => m.Map<IEnumerable<GameDTO>>(games))
            //    .Returns(new List<GameDTO>
            //    {
            //        new() { Id = 1, Title = "Game 1" },
            //        new() { Id = 2, Title = "Game 2" }
            //    });

            //// Act
            //var result = await Controller.GetGame(new GameFilterParameters());

            // Assert
            //var okResult = result.Result as OkObjectResult;
            //okResult.Should().NotBeNull();

            //var returnedGames = okResult!.Value as IEnumerable<GameDTO>;
            //returnedGames.Should().NotBeNull();
            //returnedGames!.Count().Should().Be(2);
        }

        [Trait("GamesController", "Get")]
        [Fact]
        public async Task GetGame_ReturnsOk_WhenGameExists()
        {
            // Arrange
            //var game = new Game { Id = 1, Title = "Game 1" };
            var dto = new GameDTO { Id = 1, Title = "Game 1" };

            MockGameService
                .Setup(s => s.GetGameAsync(1))
                .ReturnsAsync(dto);

            //MockUnitOfWork.Setup(u => u.GameRepository.GetAsync(1)).ReturnsAsync(game);
            //MockMapper.Setup(m => m.Map<GameDTO>(game)).Returns(dto);

            // Act
            var result = await Controller.GetGame(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();

            var returnedGame = okResult!.Value as GameDTO;
            returnedGame.Should().NotBeNull();
            returnedGame!.Id.Should().Be(1);
            returnedGame.Title.Should().Be("Game 1");
        }

        [Trait("GamesController", "Get")]
        [Fact]
        public async Task GetGame_ReturnsNotFound_WhenGameDoesNotExist()
        {
            // Arrange
            //MockUnitOfWork.Setup(u => u.GameRepository.GetAsync(1)).ReturnsAsync((Game?)null);
            MockGameService
                .Setup(s => s.GetGameAsync(1))
                .ReturnsAsync((GameDTO?)null);

            // Act
            var result = await Controller.GetGame(1);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Trait("GamesController", "Post")]
        [Fact]
        public async Task PostGame_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = new CreateGameDTO { Title = "New Game" };
            //var gameEntity = new Game { Id = 1, Title = "New Game" };
            var gameDto = new GameDTO { Id = 1, Title = "New Game" };

            MockGameService
                .Setup(s => s.CreateGameAsync(createDto))
                .ReturnsAsync(gameDto);

            //MockMapper.Setup(m => m.Map<Game>(createDto)).Returns(gameEntity);
            //MockMapper.Setup(m => m.Map<GameDTO>(gameEntity)).Returns(gameDto);
            //MockUnitOfWork.Setup(u => u.GameRepository.Add(gameEntity));
            //MockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await Controller.PostGame(createDto);

            // Assert
            var createdAtAction = result.Result as CreatedAtActionResult;
            createdAtAction
                .Should().NotBeNull();
            createdAtAction!.ActionName.Should().Be(nameof(GamesController.GetGame));

            var returnedDto = createdAtAction.Value as GameDTO;
            returnedDto.Should().NotBeNull();
            returnedDto!.Title.Should().Be("New Game");
        }

        [Trait("GamesController", "Delete")]
        [Fact]
        public async Task DeleteGame_ReturnsNoContent_WhenGameExists()
        {
            // Arrange
            var game = new Game { Id = 1, Title = "Game to delete" };

            MockGameService
                .Setup(s => s.DeleteGameAsync(1))
                .ReturnsAsync(true);
            //MockUnitOfWork.Setup(u => u.GameRepository.GetAsync(1)).ReturnsAsync(game);
            //MockUnitOfWork.Setup(u => u.GameRepository.Remove(game));
            //MockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await Controller.DeleteGame(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Trait("GamesController", "Delete")]
        [Fact]
        public async Task DeleteGame_ReturnsNotFound_WhenGameDoesNotExist()
        {
            // Arrange
            //MockUnitOfWork.Setup(u => u.GameRepository.GetAsync(1)).ReturnsAsync((Game?)null);
            MockGameService
                .Setup(s => s.DeleteGameAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await Controller.DeleteGame(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Trait("GamesController", "Put")]
        [Fact]
        public async Task PutGame_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var updateDto = new UpdateGameDTO { Title = "Updated Title" };
            //var gameEntity = new Game { Id = 1, Title = "Old Title" };

            MockGameService
                .Setup(s => s.UpdateGameAsync(1, updateDto))
                .ReturnsAsync(true);

            //MockUnitOfWork.Setup(u => u.GameRepository.AnyAsync(1)).ReturnsAsync(true);
            //MockUnitOfWork.Setup(u => u.GameRepository.GetAsync(1)).ReturnsAsync(gameEntity);
            //MockMapper.Setup(m => m.Map(updateDto, gameEntity));
            //MockUnitOfWork.Setup(u => u.GameRepository.Update(gameEntity));
            //MockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await Controller.PutGame(1, updateDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Trait("GamesController", "Put")]
        [Fact]
        public async Task PutGame_ReturnsNotFound_WhenGameDoesNotExist()
        {
            // Arrange
            //MockUnitOfWork.Setup(u => u.GameRepository.AnyAsync(1)).ReturnsAsync(false);
            MockGameService
                .Setup(s => s.UpdateGameAsync(1, It.IsAny<UpdateGameDTO>()))
                .ReturnsAsync(false);

            // Act
            var result = await Controller.PutGame(1, new UpdateGameDTO { Title = "Test" });

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Trait("GamesController", "Patch")]
        [Fact]
        public async Task PatchGame_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var patchDoc = new JsonPatchDocument<GameDTO>();
            patchDoc.Replace(x => x.Title, "New Title");

            var updatedDto = new GameDTO { Id = 1, Title = "New Title" };

            MockGameService
                .Setup(s => s.PatchGameAsync(1, patchDoc))
                .ReturnsAsync(updatedDto);
            //var entity = new Game { Id = 1, Title = "Old Title" };
            //var dto = new GameDTO { Id = 1, Title = "Old Title" };

            //MockUnitOfWork.Setup(u => u.GameRepository.GetAsync(1))
            //    .ReturnsAsync(entity);

            //MockMapper.Setup(m => m.Map<GameDTO>(entity))
            //    .Returns(dto);

            //MockMapper.Setup(m => m.Map(It.IsAny<GameDTO>(), entity))
            //    .Callback<GameDTO, Game>((src, dest) => 
            //    { 
            //        dest.Title = src.Title;
            //    });

            //MockUnitOfWork.Setup(u => u.CompleteAsync())
            //    .Returns(Task.CompletedTask);

            //var patchDoc = new JsonPatchDocument<GameDTO>();
            //patchDoc.ContractResolver = new DefaultContractResolver();
            //patchDoc.Replace(x => x.Title, "New Title");

            // Act
            var result = await Controller.PatchGame(1, patchDoc);

            // Assert
            //result.Result.Should().BeOfType<OkObjectResult>();
            //var okResult = result.Result as OkObjectResult;
            //okResult.Should().NotBeNull();

            //var returnedDto = okResult!.Value as GameDTO;
            //returnedDto.Should().NotBeNull();
            //returnedDto!.Title.Should().Be("New Title");

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();

            var returnedDto = okResult!.Value as GameDTO;
            returnedDto.Should().NotBeNull();
            returnedDto!.Title.Should().Be("New Title");
        }

        [Trait("GamesController", "Validation")]
        [Fact]
        public async Task PostGame_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var invalidDto = new CreateGameDTO { Title = null! };
            Controller.ModelState.AddModelError("Title", "Title is required.");

            // Act
            var result = await Controller.PostGame(invalidDto);

            // Assert
            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.StatusCode.Should().Be(400);
        }

        [Trait("GamesController", "Validation")]
        [Fact]
        public async Task PutGame_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var invalidDto = new UpdateGameDTO { Title = null! };
            Controller.ModelState.AddModelError("Title", "Title is required.");

            // Act
            var result = await Controller.PutGame(1, invalidDto);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.StatusCode.Should().Be(400);
        }
    }
}

