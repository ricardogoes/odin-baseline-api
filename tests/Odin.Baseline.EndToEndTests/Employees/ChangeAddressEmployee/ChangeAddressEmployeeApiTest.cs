using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Employees.ChangeAddressEmployee;
using Odin.Baseline.Application.Employees.Common;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Employees.ChangeAddressEmployee
{

    [Collection(nameof(ChangeAddressEmployeeApiTestCollection))]
    public class ChangeAddressEmployeeApiTest
    {
        private readonly ChangeAddressEmployeeApiTestFixture _fixture;

        public ChangeAddressEmployeeApiTest(ChangeAddressEmployeeApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should change a employee address")]
        [Trait("E2E/Controllers", "Employees / [v1]ChangeAddressEmployee")]
        public async Task ActivateEmployee()
        {
            var customer = _fixture.GetValidCustomerModel();
            var department = _fixture.GetValidDepartmentModel();

            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customer.Id }, new List<Guid> { department.Id }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddAsync(customer);
            await dbContext.AddAsync(department);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeToChangeAddress = employeesList.Where(x => x.IsActive).First();

            var input = _fixture.GetValidInput(employeeToChangeAddress.Id) ;

            var (response, output) = await _fixture.ApiClient.PutAsync<EmployeeOutput>($"/v1/employees/{employeeToChangeAddress.Id}/addresses", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.FirstName.Should().Be(employeeToChangeAddress.FirstName);
            output.LastName.Should().Be(employeeToChangeAddress.LastName);
            output.Document.Should().Be(employeeToChangeAddress.Document);
            output.Email.Should().Be(employeeToChangeAddress.Email);
            output.IsActive.Should().BeTrue();
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);

            output.Address.Should().NotBeNull();
            output.Address!.StreetName.Should().Be(input.StreetName);
            output.Address.StreetNumber.Should().Be(input.StreetNumber);
            output.Address.Complement.Should().Be(input.Complement);
            output.Address.Neighborhood.Should().Be(input.Neighborhood);
            output.Address.ZipCode.Should().Be(input.ZipCode);
            output.Address.City.Should().Be(input.City);
            output.Address.State.Should().Be(input.State);
        }

        [Fact(DisplayName = "Should throw an error when Id is empty")]
        [Trait("E2E/Controllers", "Employees / [v1]ChangeAddressEmployee")]
        public async Task ErrorWhenInvalidIds()
        {

            var input = _fixture.GetValidInput();
            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/employees/{Guid.Empty}/addresses", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Fact(DisplayName = "Should throw an error when employee not found")]
        [Trait("E2E/Controllers", "Employees / [v1]ChangeAddressEmployee")]
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

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/employees/{idToQuery}/addresses", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"Employee with Id '{idToQuery}' not found.");
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Employees / [v1]ChangeAddressEmployee")]
        [MemberData(
            nameof(ChangeAddressEmployeeApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(ChangeAddressEmployeeApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiateAddress(ChangeAddressEmployeeInput input, string expectedDetail)
        {
            var customer = _fixture.GetValidCustomerModel();
            var department = _fixture.GetValidDepartmentModel();
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customer.Id }, new List<Guid> { department.Id }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddAsync(customer);
            await dbContext.AddAsync(department);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = employeesList.Where(x => x.IsActive).Select(x => x.Id).FirstOrDefault();
            input.ChangeEmployeeId(idToQuery);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/employees/{idToQuery}/addresses", input);

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
