using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Positions;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Controllers.Positions.ChangeStatusPosition
{

    [Collection(nameof(ChangeStatusPositionApiTestCollection))]
    public class ChangeStatusPositionApiTest
    {
        private readonly ChangeStatusPositionApiTestFixture _fixture;

        public ChangeStatusPositionApiTest(ChangeStatusPositionApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should activate a position")]
        [Trait("E2E/Controllers", "Positions / [v1]ChangeStatusPosition")]
        public async Task ActivatePosition()
        {
            var positionsList = _fixture.GetValidPositionsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(positionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var positionToChangeStatus = positionsList.Where(x => !x.IsActive).First();

            var input = _fixture.GetValidInputToActivate(positionToChangeStatus.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<PositionOutput>($"/v1/positions/{positionToChangeStatus.Id}/status?action=ACTIVATE", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(positionToChangeStatus.Name);
            output.IsActive.Should().BeTrue();
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should deactivate a position")]
        [Trait("E2E/Controllers", "Positions / [v1]ChangeStatusPosition")]
        public async Task DeactivatePosition()
        {
            var positionsList = _fixture.GetValidPositionsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(positionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var positionToChangeStatus = positionsList.Where(x => x.IsActive).First();

            var input = _fixture.GetValidInputToDeactivate(positionToChangeStatus.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<PositionOutput>($"/v1/positions/{positionToChangeStatus.Id}/status?action=DEACTIVATE", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(positionToChangeStatus.Name);
            output.IsActive.Should().BeFalse();
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should throw an error when Id is empty")]
        [Trait("E2E/Controllers", "Positions / [v1]ChangeStatusPosition")]
        public async Task ErrorWhenInvalidIds()
        {
            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/positions/{Guid.Empty}/status?action=ACTIVATE", null);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Fact(DisplayName = "Should throw an error when action is invalid")]
        [Trait("E2E/Controllers", "Positions / [v1]ChangeStatusPosition")]
        public async Task ErrorWhenInvalidAction()
        {

            var positionToChangeStatus = _fixture.GetValidPosition();

            var input = _fixture.GetInputWithInvalidAction(positionToChangeStatus.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/positions/{Guid.NewGuid()}/status?action=INVALID", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid action. Only ACTIVATE or DEACTIVATE values are allowed");
        }


        [Fact(DisplayName = "Should throw an error when position not found")]
        [Trait("E2E/Controllers", "Positions / [v1]ChangeStatusPosition")]
        public async Task ErrorWhenNotFound()
        {
            var positionsList = _fixture.GetValidPositionsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(positionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();
            var input = _fixture.GetValidInputToActivate(idToQuery);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/positions/{idToQuery}/status?action=ACTIVATE", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"Position with Id '{idToQuery}' not found.");
        }
    }
}
