using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Positions;
using Odin.Baseline.Application.Positions.UpdatePosition;
using System.Net;
using System.Text.Json;

namespace Odin.Baseline.EndToEndTests.Controllers.Positions.UpdatePosition
{

    [Collection(nameof(UpdatePositionApiTestCollection))]
    public class UpdatePositionApiTest
    {
        private readonly UpdatePositionApiTestFixture _fixture;

        public UpdatePositionApiTest(UpdatePositionApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should update a valid position")]
        [Trait("E2E/Controllers", "Positions / [v1]UpdatePosition")]
        public async Task UpdateValidPosition()
        {
            var positionsList = _fixture.GetValidPositionsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(positionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var positionToUpdate = positionsList[10];

            var input = _fixture.GetValidInput(positionToUpdate.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<PositionOutput>($"/v1/positions/{positionToUpdate.Id}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.IsActive.Should().Be(positionToUpdate.IsActive);
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should throw an error when Id from route is different of Id from position")]
        [Trait("E2E/Controllers", "Positions / [v1]UpdatePosition")]
        public async Task ErrorWhenInvalidIds()
        {
            var positionsList = _fixture.GetValidPositionsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(positionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var positionToUpdate = positionsList[10];

            var input = _fixture.GetValidInput(positionToUpdate.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/positions/{Guid.NewGuid()}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Positions / [v1]UpdatePosition")]
        [MemberData(
            nameof(UpdatePositionApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(UpdatePositionApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiatePosition(UpdatePositionInput input, string property, string expectedDetail)
        {
            var positionsList = _fixture.GetValidPositionsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(positionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var positionToUpdate = positionsList[10];
            input.ChangeId(positionToUpdate.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/positions/{input.Id}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            output.Should().NotBeNull();
            output.Title.Should().Be("Unprocessable entity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);

            output.Extensions["errors"].Should().NotBeNull();
            var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(JsonSerializer.Serialize(output.Extensions["errors"]))!;
            errors.ContainsKey(property).Should().BeTrue();
            errors[property].First().Should().Be(expectedDetail);
        }

        [Fact(DisplayName = "Should throw an error when position not found")]
        [Trait("E2E/Controllers", "Positions / [v1]UpdatePosition")]
        public async Task ErrorWhenNotFound()
        {
            var positionsList = _fixture.GetValidPositionsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(positionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();
            var input = _fixture.GetValidInput(idToQuery);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/positions/{idToQuery}", input);

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
