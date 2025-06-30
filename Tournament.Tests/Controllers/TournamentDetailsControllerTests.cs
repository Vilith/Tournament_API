using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Api.Parameters;
using Tournament.Core.DTO;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Controllers;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Tournament.Tests.TestHelpers;

namespace Tournament.Tests.Controllers
{
    public class TournamentDetailsControllerTests : ControllerTestBase<TournamentDetailsController>
    {
        public TournamentDetailsControllerTests() : base((uow, mapper) => new TournamentDetailsController(uow, mapper))
        {            
        }

        [Trait("TournamentDetailsController", "Get")]
        [Fact]
        public async Task GetTournamentDetails_ReturnsOkWithDTOs()
        {
            // Arrange
            var parameters = new TournamentFilterParameters();
            var tournaments = new List<TournamentDetails> { new TournamentDetails { Id = 1, Title = "Test" } };
            var dtoList = new List<TournamentDTO> { new TournamentDTO { Id = 1, Title = "Test" } };

            MockUnitOfWork.Setup(u => u.TournamentRepository.GetFilteredAsync(parameters)).ReturnsAsync(tournaments);
            MockMapper.Setup(m => m.Map<IEnumerable<TournamentDTO>>(tournaments)).Returns(dtoList);

            // Act
            var result = await Controller.GetTournamentDetails(parameters);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(dtoList);
        }

        [Trait("TournamentDetailsController", "Get")]
        [Fact]
        public async Task GetTournamentDetails_ById_ReturnsOk_WhenFound()
        {
            var tournament = new TournamentDetails { Id = 1, Title = "Test" };
            var dto = new TournamentDTO { Id = 1, Title = "Test" };

            MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync(tournament);
            MockMapper.Setup(m => m.Map<TournamentDTO>(tournament)).Returns(dto);

            var result = await Controller.GetTournamentDetails(1);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(dto);
        }

        [Trait("TournamentDetailsController", "Get")]
        [Fact]
        public async Task GetTournamentDetails_ById_ReturnsNotFound_WhenNotFound()
        {
            MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync((TournamentDetails)null!);

            var result = await Controller.GetTournamentDetails(1);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Trait("TournamentDetailsController", "Post")]
        [Fact]
        public async Task PostTournamentDetails_ReturnsCreatedAtAction()
        {
            var createDto = new CreateTournamentDTO { Title = "Test" };
            var tournament = new TournamentDetails { Id = 1, Title = "Test" };
            var dto = new TournamentDTO { Id = 1, Title = "Test" };

            MockMapper.Setup(m => m.Map<TournamentDetails>(createDto)).Returns(tournament);
            MockMapper.Setup(m => m.Map<TournamentDTO>(tournament)).Returns(dto);

            MockUnitOfWork.Setup(u => u.TournamentRepository.Add(tournament));
            MockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            var result = await Controller.PostTournamentDetails(createDto);

            var createdAt = result.Result as CreatedAtActionResult;
            createdAt.Should().NotBeNull();
            createdAt!.ActionName.Should().Be(nameof(Controller.GetTournamentDetails));
            createdAt.Value.Should().BeEquivalentTo(dto);
        }

        [Trait("TournamentDetailsController", "Put")]
        [Fact]
        public async Task PutTournamentDetails_ReturnsNoContent_WhenSuccess()
        {
            var updateDto = new UpdateTournamentDTO { Title = "Updated" };
            var entity = new TournamentDetails { Id = 1, Title = "Old" };

            MockUnitOfWork.Setup(u => u.TournamentRepository.AnyAsync(1)).ReturnsAsync(true);
            MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync(entity);
            MockMapper.Setup(m => m.Map(updateDto, entity));

            MockUnitOfWork.Setup(u => u.TournamentRepository.Update(entity));
            MockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            var result = await Controller.PutTournamentDetails(1, updateDto);

            result.Should().BeOfType<NoContentResult>();
        }

        [Trait("TournamentDetailsController", "Put")]
        [Fact]
        public async Task PutTournamentDetails_ReturnsNotFound_WhenNotFound()
        {
            var updateDto = new UpdateTournamentDTO { Title = "Updated" };

            MockUnitOfWork.Setup(u => u.TournamentRepository.AnyAsync(1)).ReturnsAsync(false);

            var result = await Controller.PutTournamentDetails(1, updateDto);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Trait("TournamentDetailsController", "Delete")]
        [Fact]
        public async Task DeleteTournamentDetails_ReturnsNoContent_WhenSuccess()
        {
            var entity = new TournamentDetails { Id = 1, Title = "Test" };

            MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync(entity);
            MockUnitOfWork.Setup(u => u.TournamentRepository.Remove(entity));
            MockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            var result = await Controller.DeleteTournamentDetails(1);

            result.Should().BeOfType<NoContentResult>();
        }

        [Trait("TournamentDetailsController", "Delete")]
        [Fact]
        public async Task DeleteTournamentDetails_ReturnsNotFound_WhenNotFound()
        {
            MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync((TournamentDetails)null!);

            var result = await Controller.DeleteTournamentDetails(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Trait("TournamentDetailsController", "Patch")]
        [Fact]
        public async Task PatchTournament_ReturnsBadRequest_WhenPatchDocumentIsNull()
        {
            var result = await Controller.PatchTournament(1, null!);

            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().Be("Patch document cannot be null.");
        }

        [Trait("TournamentDetailsController", "Patch")]
        [Fact]
        public async Task PatchTournament_ReturnsNotFound_WhenTournamentNotFound()
        {
            MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync((TournamentDetails)null!);
            var patchDoc = new JsonPatchDocument<TournamentDTO>();

            var result = await Controller.PatchTournament(1, patchDoc);

            var notFound = result.Result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound!.Value.Should().Be("Tournament with ID 1 not found.");
        }

        [Trait("TournamentDetailsController", "Patch")]
        [Fact]
        public async Task PatchTournament_ReturnsOk_WhenSuccessful()
        {
            var entity = new TournamentDetails { Id = 1, Title = "Old" };
            var dto = new TournamentDTO { Id = 1, Title = "Old" };

            MockUnitOfWork.Setup(u => u.TournamentRepository.GetAsync(1, It.IsAny<bool>())).ReturnsAsync(entity);

            MockMapper.Setup(m => m.Map<TournamentDTO>(entity)).Returns(dto);
            MockMapper.Setup(m => m.Map(It.IsAny<TournamentDTO>(), entity))
                .Callback<TournamentDTO, TournamentDetails>((src, dest) => dest.Title = src.Title);

            MockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            var patchDoc = new JsonPatchDocument<TournamentDTO>();
            patchDoc.ContractResolver = new DefaultContractResolver();
            patchDoc.Replace(x => x.Title, "New");                       

            var result = await Controller.PatchTournament(1, patchDoc);
            
            result.Value.Should().NotBeNull();
            result.Value.Title.Should().Be("New");
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

