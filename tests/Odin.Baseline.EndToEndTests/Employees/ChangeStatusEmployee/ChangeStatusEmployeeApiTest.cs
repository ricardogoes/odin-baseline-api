using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Employees.Common;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Employees.ChangeStatusEmployee
{

    [Collection(nameof(ChangeStatusEmployeeApiTestCollection))]
    public class ChangeStatusEmployeeApiTest
    {
        private readonly ChangeStatusEmployeeApiTestFixture _fixture;

        public ChangeStatusEmployeeApiTest(ChangeStatusEmployeeApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should activate a employee")]
        [Trait("E2E/Controllers", "Employees / [v1]ChangeStatusEmployee")]
        public async Task ActivateEmployee()
        {
            var customerId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customerId }, new List<Guid> { departmentId }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeToChangeStatus = employeesList.Where(x => !x.IsActive).FirstOrDefault();

            var input = _fixture.GetValidInputToActivate(employeeToChangeStatus.Id) ;

            var (response, output) = await _fixture.ApiClient.PutAsync<EmployeeOutput>($"/v1/employees/{employeeToChangeStatus.Id}/status?action=ACTIVATE", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.FirstName.Should().Be(employeeToChangeStatus.FirstName);
            output.LastName.Should().Be(employeeToChangeStatus.LastName);
            output.Document.Should().Be(employeeToChangeStatus.Document);
            output.Email.Should().Be(employeeToChangeStatus.Email);
            output.IsActive.Should().BeTrue();
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should deactivate a employee")]
        [Trait("E2E/Controllers", "Employees / [v1]ChangeStatusEmployee")]
        public async Task DeactivateEmployee()
        {
            var customerId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customerId }, new List<Guid> { departmentId }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeToChangeStatus = employeesList.Where(x => x.IsActive).FirstOrDefault();

            var input = _fixture.GetValidInputToDeactivate(employeeToChangeStatus.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<EmployeeOutput>($"/v1/employees/{employeeToChangeStatus.Id}/status?action=DEACTIVATE", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.FirstName.Should().Be(employeeToChangeStatus.FirstName);
            output.LastName.Should().Be(employeeToChangeStatus.LastName);
            output.Document.Should().Be(employeeToChangeStatus.Document);
            output.Email.Should().Be(employeeToChangeStatus.Email);
            output.IsActive.Should().BeFalse();
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should throw an error when Id is empty")]
        [Trait("E2E/Controllers", "Employees / [v1]ChangeStatusEmployee")]
        public async Task ErrorWhenInvalidIds()
        {            
            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/employees/{Guid.Empty}/status?action=ACTIVATE", null);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Fact(DisplayName = "Should throw an error when action is invalid")]
        [Trait("E2E/Controllers", "Employees / [v1]ChangeStatusEmployee")]
        public async Task ErrorWhenInvalidAction()
        {
            var employeeToChangeStatus = _fixture.GetValidEmployee();

            var input = _fixture.GetInputWithInvalidAction(employeeToChangeStatus.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/employees/{Guid.NewGuid()}/status?action=INVALID", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid action. Only ACTIVATE or DEACTIVATE values are allowed");
        }


        [Fact(DisplayName = "Should throw an error when employee not found")]
        [Trait("E2E/Controllers", "Employees / [v1]ChangeStatusEmployee")]
        public async Task ErrorWhenNotFound()
        {
            var customerId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customerId }, new List<Guid> { departmentId }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();
            var input = _fixture.GetValidInputToActivate(idToQuery);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/employees/{idToQuery}/status?action=ACTIVATE", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"Employee with Id '{idToQuery}' not found.");
        }
    }
}
