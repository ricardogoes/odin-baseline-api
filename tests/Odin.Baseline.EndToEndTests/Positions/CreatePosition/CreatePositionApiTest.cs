using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Application.Positions.CreatePosition;
using System.Net;

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
            var input = _fixture.GetValidInput();

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
        public async Task ErrorWhenCantInstantiatePosition(CreatePositionInput input, string expectedDetail)
        {
            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>("/v1/positions", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            output.Should().NotBeNull();
            output.Title.Should().Be("One or more validation errors ocurred");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
            output.Detail.Should().Be(expectedDetail);
        }
    }
}
