using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Application.Departments.CreateDepartment;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Departments.CreateDepartment
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
            var customer = _fixture.GetValidCustomerModel();
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddAsync(customer);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = _fixture.GetValidInput(customer.Id);

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
        public async Task ErrorWhenCantInstantiateDepartment(CreateDepartmentInput input, string expectedDetail)
        {
            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>("/v1/departments", input);

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
