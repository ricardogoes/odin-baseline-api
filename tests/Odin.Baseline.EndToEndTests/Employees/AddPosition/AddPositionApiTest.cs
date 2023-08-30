using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Employees.AddPosition;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.EndToEndTests.Employees.AddPosition;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Employees.AddPosition
{

    [Collection(nameof(AddPositionApiTestCollection))]
    public class AddPositionApiTest
    {
        private readonly AddPositionApiTestFixture _fixture;

        public AddPositionApiTest(AddPositionApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should add a position")]
        [Trait("E2E/Controllers", "Employees / [v1]AddPosition")]
        public async Task AddPositionToEmployee()
        {
            var customerId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customerId }, new List<Guid> { departmentId }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeToAddPosition = employeesList.Where(x => x.IsActive).FirstOrDefault();

            var input = _fixture.GetValidInput(employeeToAddPosition.Id) ;

            var (response, output) = await _fixture.ApiClient.PostAsync<EmployeeOutput>($"/v1/employees/{employeeToAddPosition.Id}/positions", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.FirstName.Should().Be(employeeToAddPosition.FirstName);
            output.LastName.Should().Be(employeeToAddPosition.LastName);
            output.Email.Should().Be(employeeToAddPosition.Email);
            output.Document.Should().Be(employeeToAddPosition.Document);
            output.IsActive.Should().BeTrue();
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);

            output.PositionsHistory.Should().NotBeNull();
            output.PositionsHistory.Should().HaveCount(1);

            var positionHistory = output.PositionsHistory.FirstOrDefault();
            positionHistory.PositionId.Should().Be(input.PositionId);
            positionHistory.Salary.Should().Be(input.Salary);
            positionHistory.StartDate.Should().NotBeSameDateAs(default);
            positionHistory.FinishDate.Should().BeNull();
            positionHistory.IsActual.Should().BeTrue();
        }

        [Fact(DisplayName = "Should throw an error when Id is empty")]
        [Trait("E2E/Controllers", "Employees / [v1]AddPosition")]
        public async Task ErrorWhenInvalidIds()
        {

            var input = _fixture.GetValidInput();
            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>($"/v1/employees/{Guid.Empty}/positions", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Fact(DisplayName = "Should throw an error when employee not found")]
        [Trait("E2E/Controllers", "Employees / [v1]AddPosition")]
        public async Task ErrorWhenNotFound()
        {
            var customerId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customerId }, new List<Guid> { departmentId }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();
            var input = _fixture.GetValidInput(idToQuery);

            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>($"/v1/employees/{idToQuery}/positions", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"Employee with Id '{idToQuery}' not found.");
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Employees / [v1]AddPosition")]
        [MemberData(
            nameof(AddPositionApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(AddPositionApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiatePosition(AddPositionInput input, string expectedDetail)
        {
            var customerId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customerId }, new List<Guid> { departmentId }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = employeesList.Where(x => x.IsActive).Select(x => x.Id).FirstOrDefault();
            input.EmployeeId = idToQuery;

            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>($"/v1/employees/{idToQuery}/positions", input);

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
