using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Departments.Common;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Departments.GetDepartmentById
{

    [Collection(nameof(GetDepartmentByIdApiTestCollection))]
    public class GetDepartmentByIdApiTest
    {
        private readonly GetDepartmentByIdApiTestFixture _fixture;

        public GetDepartmentByIdApiTest(GetDepartmentByIdApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should get a department by valid id")]
        [Trait("E2E/Controllers", "Departments / [v1]GetDepartmentById")]
        public async Task GetDepartmentById()
        {
            var departmentsList = _fixture.GetValidDepartmentsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var departmentToQuery = departmentsList[10];

            var (response, output) = await _fixture.ApiClient.GetByIdAsync<DepartmentOutput>($"/v1/departments/{departmentToQuery.Id}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Id.Should().Be(departmentToQuery.Id);
            output.Name.Should().Be(departmentToQuery.Name);
            output.IsActive.Should().Be(departmentToQuery.IsActive);
            output.CreatedAt.Should().Be(departmentToQuery.CreatedAt);
        }

        [Fact(DisplayName = "Should throw an error when department not found")]
        [Trait("E2E/Controllers", "Departments / [v1]GetDepartmentById")]
        public async Task ErrorWhenNotFound()
        {
            var departmentsList = _fixture.GetValidDepartmentsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();

            var (response, output) = await _fixture.ApiClient.GetByIdAsync<ProblemDetails>($"/v1/departments/{idToQuery}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"Department with Id '{idToQuery}' not found.");
        }
    }
}
