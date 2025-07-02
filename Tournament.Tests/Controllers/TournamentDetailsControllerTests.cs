using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using Newtonsoft.Json.Serialization;
using Tournament.Tests.TestHelpers;
using Tournament.Presentation.Controllers;
using Tournament.Shared.Parameters;
using Domain.Models.Entities;
using Tournament.Shared.DTO;
using Services.Contracts;

namespace Tournament.Tests.Controllers
{
    public class TournamentDetailsControllerTests : ControllerTestBase<TournamentDetailsController>
    {
        private readonly Mock<ITournamentService> MockTournamentService = new();
        public TournamentDetailsControllerTests() : base((serviceManager) => new TournamentDetailsController(serviceManager))
        {       
            MockService.Setup(s => s.TournamentService).Returns(MockTournamentService.Object);
        }

        [Trait("TournamentDetailsController", "Get")]
        [Fact]
        public async Task GetTournamentDetails_ReturnsOkWithDTOs()
        {
            var parameters = new TournamentFilterParameters();
            var dtoList = new List<TournamentDTO> { new TournamentDTO { Id = 1, Title = "Test" } };

            MockTournamentService.Setup(s => s.GetTournamentDetailsAsync(parameters))
                .ReturnsAsync(dtoList);

            var result = await Controller.GetTournamentDetails(parameters);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(dtoList);

            //// Arrange
            //var parameters = new TournamentFilterParameters();
            //var tournaments = new List<TournamentDetails> { new TournamentDetails { Id = 1, Title = "Test" } };
            //var dtoList = new List<TournamentDTO> { new TournamentDTO { Id = 1, Title = "Test" } };

            //MockUnitOfWork.Setup(u => u.TournamentRepository.GetFilteredAsync(parameters)).ReturnsAsync(tournaments);
            //MockMapper.Setup(m => m.Map<IEnumerable<TournamentDTO>>(tournaments)).Returns(dtoList);

            //// Act
            //var result = await Controller.GetTournamentDetails(parameters);

            //// Assert
            //var okResult = result.Result as OkObjectResult;
            //okResult.Should().NotBeNull();
            //okResult!.Value.Should().BeEquivalentTo(dtoList);
        }

        [Trait("TournamentDetailsController", "Get")]
        [Fact]
        public async Task GetTournamentDetails_ById_ReturnsOk_WhenFound()
        {
            var dto = new TournamentDTO { Id = 1, Title = "Test" };

            MockTournamentService.Setup(s => s.GetTournamentDetailsAsync(1))
                .ReturnsAsync(dto);

            var result = await Controller.GetTournamentDetails(1);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(dto);

            //var tournament = new TournamentDetails { Id = 1, Title = "Test" };
            //var dto = new TournamentDTO { Id = 1, Title = "Test" };

            //MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync(tournament);
            //MockMapper.Setup(m => m.Map<TournamentDTO>(tournament)).Returns(dto);

            //var result = await Controller.GetTournamentDetails(1);

            //var okResult = result.Result as OkObjectResult;
            //okResult.Should().NotBeNull();
            //okResult!.Value.Should().BeEquivalentTo(dto);
        }

        [Trait("TournamentDetailsController", "Get")]
        [Fact]
        public async Task GetTournamentDetails_ById_ReturnsNotFound_WhenNotFound()
        {
            MockTournamentService.Setup(s => s.GetTournamentDetailsAsync(1))
                .ReturnsAsync((TournamentDTO?)null);

            var result = await Controller.GetTournamentDetails(1);

            var notFound = result.Result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound!.StatusCode.Should().Be(404);
            notFound.Value.Should().Be("Tournament with ID 1 not found.");
            //result.Result.Should().BeOfType<NotFoundResult>();

            //MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync((TournamentDetails)null!);

            //var result = await Controller.GetTournamentDetails(1);

            //result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Trait("TournamentDetailsController", "Post")]
        [Fact]
        public async Task PostTournamentDetails_ReturnsCreatedAtAction()
        {
            var createDto = new CreateTournamentDTO { Title = "Test" };
            var dto = new TournamentDTO { Id = 1, Title = "Test" };

            MockTournamentService.Setup(s => s.CreateTournamentDetailsAsync(createDto))
                .ReturnsAsync(dto);

            var result = await Controller.PostTournamentDetails(createDto);

            var createdAt = result.Result as CreatedAtActionResult;
            createdAt.Should().NotBeNull();
            createdAt!.ActionName.Should().Be(nameof(Controller.GetTournamentDetails));
            createdAt.Value.Should().BeEquivalentTo(dto);


            //var createDto = new CreateTournamentDTO { Title = "Test" };
            //var tournament = new TournamentDetails { Id = 1, Title = "Test" };
            //var dto = new TournamentDTO { Id = 1, Title = "Test" };

            //MockMapper.Setup(m => m.Map<TournamentDetails>(createDto)).Returns(tournament);
            //MockMapper.Setup(m => m.Map<TournamentDTO>(tournament)).Returns(dto);

            //MockUnitOfWork.Setup(u => u.TournamentRepository.Add(tournament));
            //MockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            //var result = await Controller.PostTournamentDetails(createDto);

            //var createdAt = result.Result as CreatedAtActionResult;
            //createdAt.Should().NotBeNull();
            //createdAt!.ActionName.Should().Be(nameof(Controller.GetTournamentDetails));
            //createdAt.Value.Should().BeEquivalentTo(dto);
        }

        [Trait("TournamentDetailsController", "Put")]
        [Fact]
        public async Task PutTournamentDetails_ReturnsNoContent_WhenSuccess()
        {
            var updateDto = new UpdateTournamentDTO { Title = "Updated" };

            MockTournamentService.Setup(s => s.UpdateTournamentDetailsAsync(1, updateDto))
                .ReturnsAsync(true);

            var result = await Controller.PutTournamentDetails(1, updateDto);

            result.Should().BeOfType<NoContentResult>();


            //var updateDto = new UpdateTournamentDTO { Title = "Updated" };
            //var entity = new TournamentDetails { Id = 1, Title = "Old" };

            //MockUnitOfWork.Setup(u => u.TournamentRepository.AnyAsync(1)).ReturnsAsync(true);
            //MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync(entity);
            //MockMapper.Setup(m => m.Map(updateDto, entity));

            //MockUnitOfWork.Setup(u => u.TournamentRepository.Update(entity));
            //MockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            //var result = await Controller.PutTournamentDetails(1, updateDto);

            //result.Should().BeOfType<NoContentResult>();
        }

        [Trait("TournamentDetailsController", "Put")]
        [Fact]
        public async Task PutTournamentDetails_ReturnsNotFound_WhenNotFound()
        {           

            MockTournamentService.Setup(s => s.UpdateTournamentDetailsAsync(1, It.IsAny<UpdateTournamentDTO>()))
                .ReturnsAsync(false);

            var result = await Controller.PutTournamentDetails(1, new UpdateTournamentDTO { Title = "Test" });

            result.Should().BeOfType<NotFoundObjectResult>();

            //var updateDto = new UpdateTournamentDTO { Title = "Updated" };

            //MockUnitOfWork.Setup(u => u.TournamentRepository.AnyAsync(1)).ReturnsAsync(false);

            //var result = await Controller.PutTournamentDetails(1, updateDto);

            //result.Should().BeOfType<NotFoundResult>();
        }

        [Trait("TournamentDetailsController", "Delete")]
        [Fact]
        public async Task DeleteTournamentDetails_ReturnsNoContent_WhenSuccess()
        {
            MockTournamentService.Setup(s => s.DeleteTournamentDetailsAsync(1))
                .ReturnsAsync(true);

            var result = await Controller.DeleteTournamentDetails(1);

            result.Should().BeOfType<NoContentResult>();

            //var entity = new TournamentDetails { Id = 1, Title = "Test" };

            //MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync(entity);
            //MockUnitOfWork.Setup(u => u.TournamentRepository.Remove(entity));
            //MockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            //var result = await Controller.DeleteTournamentDetails(1);

            //result.Should().BeOfType<NoContentResult>();
        }

        [Trait("TournamentDetailsController", "Delete")]
        [Fact]
        public async Task DeleteTournamentDetails_ReturnsNotFound_WhenNotFound()
        {
            MockTournamentService
                .Setup(s => s.DeleteTournamentDetailsAsync(1))
                .ReturnsAsync(false);

            var result = await Controller.DeleteTournamentDetails(1);

            var notFound = result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound!.StatusCode.Should().Be(404);
            notFound.Value.Should().Be("Tournament with ID 1 not found.");
            //result.Should().BeOfType<NotFoundResult>();

            //MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync((TournamentDetails)null!);

            //var result = await Controller.DeleteTournamentDetails(1);

            //result.Should().BeOfType<NotFoundResult>();
        }

        [Trait("TournamentDetailsController", "Patch")]
        [Fact]
        public async Task PatchTournament_ReturnsBadRequest_WhenPatchDocumentIsNull()
        {
            var result = await Controller.PatchTournament(1, null!);

            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().Be("Patch document cannot be null.");

            //var result = await Controller.PatchTournament(1, null!);

            //var badRequest = result.Result as BadRequestObjectResult;
            //badRequest.Should().NotBeNull();
            //badRequest!.Value.Should().Be("Patch document cannot be null.");
        }

        [Trait("TournamentDetailsController", "Patch")]
        [Fact]
        public async Task PatchTournament_ReturnsNotFound_WhenTournamentNotFound()
        {
            var patchDoc = new JsonPatchDocument<TournamentDTO>();

            MockTournamentService.Setup(s => s.PatchTournamentDetailsAsync(1, patchDoc))
                .ReturnsAsync((TournamentDTO?)null);

            var result = await Controller.PatchTournament(1, patchDoc);

            var notFound = result.Result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound!.Value.Should().Be($"Tournament with ID 1 not found.");

            //MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync((TournamentDetails)null!);
            //var patchDoc = new JsonPatchDocument<TournamentDTO>();

            //var result = await Controller.PatchTournament(1, patchDoc);

            //var notFound = result.Result as NotFoundObjectResult;
            //notFound.Should().NotBeNull();
            //notFound!.Value.Should().Be("Tournament with ID 1 not found.");
        }

        [Trait("TournamentDetailsController", "Patch")]
        [Fact]
        public async Task PatchTournament_ReturnsOk_WhenSuccessful()
        {
            var patchDoc = new JsonPatchDocument<TournamentDTO>();            
            patchDoc.Replace(x => x.Title, "New Title");

            var updatedDto = new TournamentDTO { Id = 1, Title = "New Title" };

            MockTournamentService.Setup(s => s.PatchTournamentDetailsAsync(1, patchDoc))
                .ReturnsAsync(updatedDto);

            var result = await Controller.PatchTournament(1, patchDoc);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();

            var returnedDto = okResult!.Value as TournamentDTO;
            returnedDto.Should().NotBeNull();
            returnedDto!.Title.Should().Be("New Title");
            
            //result.Value.Should().NotBeNull();
            //result.Value.Title.Should().Be("New Title");

            //var entity = new TournamentDetails { Id = 1, Title = "Old" };
            //var dto = new TournamentDTO { Id = 1, Title = "Old" };

            //MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync(entity);

            //MockMapper.Setup(m => m.Map<TournamentDTO>(entity)).Returns(dto);
            //MockMapper.Setup(m => m.Map(It.IsAny<TournamentDTO>(), entity))
            //    .Callback<TournamentDTO, TournamentDetails>((src, dest) => dest.Title = src.Title);

            //MockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            //var patchDoc = new JsonPatchDocument<TournamentDTO>();
            //patchDoc.ContractResolver = new DefaultContractResolver();
            //patchDoc.Replace(x => x.Title, "New");                       

            //var result = await Controller.PatchTournament(1, patchDoc);

            //result.Value.Should().NotBeNull();
            //result.Value.Title.Should().Be("New");
        }

        [Trait("TournamentDetailsController", "Validation")]
        [Fact]
        public async Task PostTournamentDetails_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var invalidDto = new CreateTournamentDTO { Title = null! };
            Controller.ModelState.AddModelError("Title", "Title is required.");

            // Act
            var result = await Controller.PostTournamentDetails(invalidDto);

            // Assert
            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.StatusCode.Should().Be(400);
        }

        [Trait("TournamentDetailsController", "Validation")]
        [Fact]
        public async Task PutTournamentDetails_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var invalidDto = new UpdateTournamentDTO { Title = null! };
            Controller.ModelState.AddModelError("Title", "Title is required.");

            // Act
            var result = await Controller.PutTournamentDetails(1, invalidDto);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.StatusCode.Should().Be(400);
        }

    }
}

