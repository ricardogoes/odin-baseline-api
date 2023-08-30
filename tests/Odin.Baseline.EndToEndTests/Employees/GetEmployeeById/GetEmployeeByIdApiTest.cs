using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Employees.Common;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Employees.GetEmployeeById
{

    [Collection(nameof(GetEmployeeByIdApiTestCollection))]
    public class GetEmployeeByIdApiTest
    {
        private readonly GetEmployeeByIdApiTestFixture _fixture;

        public GetEmployeeByIdApiTest(GetEmployeeByIdApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should get a employee by valid id")]
        [Trait("E2E/Controllers", "Employees / [v1]GetEmployeeById")]
        public async Task GetEmployeeById()
        {
            var customerId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customerId }, new List<Guid> { departmentId }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeToQuery = employeesList[10];

            var (response, output) = await _fixture.ApiClient.GetByIdAsync<EmployeeOutput>($"/v1/employees/{employeeToQuery.Id}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Id.Should().Be(employeeToQuery.Id);
            output.FirstName.Should().Be(employeeToQuery.FirstName);
            output.LastName.Should().Be(employeeToQuery.LastName);
            output.Email.Should().Be(employeeToQuery.Email);
            output.Document.Should().Be(employeeToQuery.Document);
            output.IsActive.Should().Be(employeeToQuery.IsActive);
            output.CreatedAt.Should().Be(employeeToQuery.CreatedAt);
        }

        [Fact(DisplayName = "Should throw an error when employee not found")]
        [Trait("E2E/Controllers", "Employees / [v1]GetEmployeeById")]
        public async Task ErrorWhenNotFound()
        {
            var customerId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customerId }, new List<Guid> { departmentId }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();

            var (response, output) = await _fixture.ApiClient.GetByIdAsync<ProblemDetails>($"/v1/employees/{idToQuery}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"Employee with Id '{idToQuery}' not found.");
        }
    }
}
