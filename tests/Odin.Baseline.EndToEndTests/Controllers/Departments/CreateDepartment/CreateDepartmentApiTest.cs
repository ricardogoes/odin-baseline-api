using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Departments;
using Odin.Baseline.Application.Departments.CreateDepartment;
using System.Net;
using System.Text.Json;

namespace Odin.Baseline.EndToEndTests.Controllers.Departments.CreateDepartment
{

    [Collection(nameof(CreateDepartmentApiTestCollection))]
    public class CreateDepartmentApiTest
    {
        private readonly CreateDepartmentApiTestFixture _fixture;

        public CreateDepartmentApiTest(CreateDepartmentApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should insert a valid department")]
        [Trait("E2E/Controllers", "Departments / [v1]CreateDepartment")]
        public async Task InsertValidDepartment()
        {
            var input = _fixture.GetValidInput();

            var (response, output) = await _fixture.ApiClient.PostAsync<DepartmentOutput>("/v1/departments", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            output.Name.Should().Be(input.Name);
            output.IsActive.Should().BeTrue();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Departments / [v1]CreateDepartment")]
        [MemberData(
            nameof(CreateDepartmentApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(CreateDepartmentApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiateDepartment(CreateDepartmentInput input, string property, string expectedDetail)
        {
            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>("/v1/departments", input);

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
