using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Positions;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Controllers.Positions.GetPositionById
{

    [Collection(nameof(GetPositionByIdApiTestCollection))]
    public class GetPositionByIdApiTest
    {
        private readonly GetPositionByIdApiTestFixture _fixture;

        public GetPositionByIdApiTest(GetPositionByIdApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should get a position by valid id")]
        [Trait("E2E/Controllers", "Positions / [v1]GetPositionById")]
        public async Task GetPositionById()
        {
            var positionsList = _fixture.GetValidPositionsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(positionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var positionToQuery = positionsList[10];

            var (response, output) = await _fixture.ApiClient.GetByIdAsync<PositionOutput>($"/v1/positions/{positionToQuery.Id}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Id.Should().Be(positionToQuery.Id);
            output.Name.Should().Be(positionToQuery.Name);
            output.IsActive.Should().Be(positionToQuery.IsActive);
            output.CreatedAt.Should().Be(positionToQuery.CreatedAt);
        }

        [Fact(DisplayName = "Should throw an error when position not found")]
        [Trait("E2E/Controllers", "Positions / [v1]GetPositionById")]
        public async Task ErrorWhenNotFound()
        {
            var positionsList = _fixture.GetValidPositionsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(positionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();

            var (response, output) = await _fixture.ApiClient.GetByIdAsync<ProblemDetails>($"/v1/positions/{idToQuery}");

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
