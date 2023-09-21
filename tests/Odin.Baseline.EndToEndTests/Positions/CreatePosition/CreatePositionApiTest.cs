using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Models.Positions;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Application.Positions.CreatePosition;
using System.Net;
using System.Text.Json;

namespace Odin.Baseline.EndToEndTests.Positions.CreatePosition
{

    [Collection(nameof(CreatePositionApiTestCollection))]
    public class CreatePositionApiTest
    {
        private readonly CreatePositionApiTestFixture _fixture;

        public CreatePositionApiTest(CreatePositionApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should insert a valid position")]
        [Trait("E2E/Controllers", "Positions / [v1]CreatePosition")]
        public async Task InsertValidPosition()
        {
            var dbContext = _fixture.CreateDbContext();

            var customer = _fixture.GetValidCustomerModel();

            await dbContext.AddAsync(customer);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = _fixture.GetValidInput(customer.Id);

            var (response, output) = await _fixture.ApiClient.PostAsync<PositionOutput>("/v1/positions", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            output.Name.Should().Be(input.Name);
            output.IsActive.Should().BeTrue();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Positions / [v1]CreatePosition")]
        [MemberData(
            nameof(CreatePositionApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(CreatePositionApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiatePosition(CreatePositionInput input, string property, string expectedDetail)
        {
            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>("/v1/positions", input);

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
    }
}
