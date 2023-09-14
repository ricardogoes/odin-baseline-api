using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Application.Employees.UpdateEmployee;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Employees.UpdateEmployee
{

    [Collection(nameof(UpdateEmployeeApiTestCollection))]
    public class UpdateEmployeeApiTest
    {
        private readonly UpdateEmployeeApiTestFixture _fixture;

        public UpdateEmployeeApiTest(UpdateEmployeeApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should update a valid employee")]
        [Trait("E2E/Controllers", "Employees / [v1]UpdateEmployee")]
        public async Task UpdateValidEmployee()
        {
            var customer = _fixture.GetValidCustomerModel();
            var department = _fixture.GetValidDepartmentModel(customer.Id);
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customer.Id }, new List<Guid> { department.Id }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddAsync(customer);
            await dbContext.AddAsync(department);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeToUpdate = employeesList[10];

            var input = _fixture.GetValidUpdateEmployeeInput(employeeToUpdate.Id, customer.Id, department.Id) ;

            var (response, output) = await _fixture.ApiClient.PutAsync<EmployeeOutput>($"/v1/employees/{employeeToUpdate.Id}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.FirstName.Should().Be(input.FirstName);
            output.LastName.Should().Be(input.LastName);
            output.Email.Should().Be(input.Email);
            output.Document.Should().Be(input.Document);
            output.IsActive.Should().Be(employeeToUpdate.IsActive);
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should throw an error when Id from route is different of Id from employee")]
        [Trait("E2E/Controllers", "Employees / [v1]UpdateEmployee")]
        public async Task ErrorWhenInvalidIds()
        {

            var customer = _fixture.GetValidCustomerModel();
            var department = _fixture.GetValidDepartmentModel(customer.Id);
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customer.Id }, new List<Guid> { department.Id }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddAsync(customer);
            await dbContext.AddAsync(department);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeToUpdate = employeesList[10];

            var input = _fixture.GetValidUpdateEmployeeInput();

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/employees/{Guid.NewGuid()}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Employees / [v1]UpdateEmployee")]
        [MemberData(
            nameof(UpdateEmployeeApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(UpdateEmployeeApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiateEmployee(UpdateEmployeeInput input, string expectedDetail)
        {
            var customer = _fixture.GetValidCustomerModel();
            var department = _fixture.GetValidDepartmentModel(customer.Id);
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customer.Id }, new List<Guid> { department.Id }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddAsync(customer);
            await dbContext.AddAsync(department);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeToUpdate = employeesList[10];
            input.ChangeId(employeeToUpdate.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/employees/{input.Id}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            output.Should().NotBeNull();
            output.Title.Should().Be("One or more validation errors ocurred");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
            output.Detail.Should().Be(expectedDetail);
        }

        [Fact(DisplayName = "Should throw an error when employee not found")]
        [Trait("E2E/Controllers", "Employees / [v1]UpdateEmployee")]
        public async Task ErrorWhenNotFound()
        {
            var customer = _fixture.GetValidCustomerModel();
            var department = _fixture.GetValidDepartmentModel(customer.Id);
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customer.Id }, new List<Guid> { department.Id }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddAsync(customer);
            await dbContext.AddAsync(department);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();
            var input = _fixture.GetValidUpdateEmployeeInput(idToQuery);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/employees/{idToQuery}", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"Employee with Id '{idToQuery}' not found.");
        }

        [Fact(DisplayName = "Should throw an error when document is duplicated")]
        [Trait("E2E/Controllers", "Employees / [v1]UpdateEmployee")]
        public async Task ThrowErrorWithDuplicatedDocument()
        {
            var customer = _fixture.GetValidCustomerModel();
            var department = _fixture.GetValidDepartmentModel(customer.Id);
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customer.Id }, new List<Guid> { department.Id }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddAsync(customer);
            await dbContext.AddAsync(department);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeToUpdate = employeesList[10];
            var input = _fixture.GetValidUpdateEmployeeInput(employeeToUpdate.Id);
            input.ChangeDocument(employeesList[2].Document);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/employees/{input.Id}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            output.Should().NotBeNull();
            output.Title.Should().Be("One or more validation errors ocurred");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
            output.Detail.Should().Be("Document must be unique");
        }
    }
}
