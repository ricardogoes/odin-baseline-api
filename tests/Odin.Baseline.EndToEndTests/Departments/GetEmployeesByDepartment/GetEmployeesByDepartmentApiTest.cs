using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Models;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Application.Employees.GetEmployees;
using Odin.Baseline.Infra.Data.EF.Models;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Departments.GetEmployeesByDepartment
{

    [Collection(nameof(GetEmployeesByDepartmentApiTestCollection))]
    public class GetEmployeesByDepartmentApiTest
    {
        private readonly GetEmployeesByDepartmentApiTestFixture _fixture;

        public GetEmployeesByDepartmentApiTest(GetEmployeesByDepartmentApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should get employees when searched by a valid department")]
        [Trait("E2E/Controllers", "Departments / [v1]GetEmployeesByDepartment")]
        public async Task GetEmployeesByDepartment()
        {
            var customer = _fixture.GetValidCustomerModel();

            var departmentsList = _fixture.GetValidDepartmentsModelList(customer.Id, length: 2);
            var departmentsIds = departmentsList.Select(x => x.Id).ToList();

            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customer.Id }, departmentsIds, length: 8);                        

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddAsync(customer);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = departmentsList.Select(x => x.Id).FirstOrDefault();

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<EmployeeOutput>>($"/v1/departments/{idToQuery}/employees");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Should().NotBeNull();
            output.PageNumber.Should().Be(1);
            output.PageSize.Should().Be(5);
            output.TotalRecords.Should().Be(employeesList.Count/2);
            output.TotalPages.Should().Be(2);
            output.Items.Should().HaveCount(5);
        }

        [Fact(DisplayName = "Should throw an error when departmentId is empty")]
        [Trait("E2E/Controllers", "Departments / [v1]GetEmployeesByDepartment")]
        public async Task ThrowErrorWithEmptyId()
        {
            var (response, output) = await _fixture.ApiClient.GetAsync<ProblemDetails>($"/v1/departments/{Guid.Empty}/employees");

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Fact(DisplayName = "Should return empty list when no data found")]
        [Trait("E2E/Controllers", "Departments / [v1]GetEmployeesByDepartment")]
        public async Task ItemsEmptyWhenPersistenceEmpty()
        {
            _fixture.CreateDbContext(preserveData: false);
            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<EmployeeOutput>>($"/v1/departments/{Guid.NewGuid()}/employees");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.TotalRecords.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Theory(DisplayName = "Should return paginated data")]
        [Trait("E2E/Controllers", "Departments / [v1]GetEmployeesByDepartment")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(8, 2, 5, 3)]
        [InlineData(8, 3, 5, 0)]
        public async Task ListPaginated(int quantityToGenerate, int page, int pageSize, int expectedItems)
        {
            var customer = _fixture.GetValidCustomerModel();

            var departmentsList = _fixture.GetValidDepartmentsModelList(customer.Id, length: 2);
            var departmentsIds = departmentsList.Select(x => x.Id).ToList();

            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { customer.Id }, departmentsIds, length: quantityToGenerate);

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddAsync(customer);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = departmentsList.Select(x => x.Id).FirstOrDefault();

            var input = new GetEmployeesInput(page, pageSize, departmentId: idToQuery);
            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<EmployeeOutput>>($"/v1/departments/{idToQuery}/employees", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(quantityToGenerate);
            output.Items.Should().HaveCount(expectedItems);

            foreach (var outputItem in output.Items)
            {
                var employee = employeesList.FirstOrDefault(x => x.Id == outputItem.Id);
                employee.Should().NotBeNull();
                outputItem.FirstName.Should().Be(employee!.FirstName);
                outputItem.LastName.Should().Be(employee!.LastName);
                outputItem.Document.Should().Be(employee!.Document);
                outputItem.Email.Should().Be(employee!.Email);
                outputItem.IsActive.Should().Be(employee.IsActive);
            }
        }

        [Theory(DisplayName = "Should return filtered data by first name")]
        [Trait("E2E/Controllers", "Departments / [v1]GetEmployeesByDepartment")]
        [InlineData("Employee", 1, 5, 5, 5)]
        [InlineData("Depto", 1, 5, 3, 3)]
        [InlineData("Invalid", 1, 5, 0, 0)]
        public async Task SearchByName(string search, int page, int pageSize, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
        {
            var customer = _fixture.GetValidCustomerModel();
            var department = _fixture.GetValidDepartmentModel(customer.Id);

            var employees = new List<EmployeeModel>()
            {
                new EmployeeModel(Guid.NewGuid(), "Employee 01", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Employee 02", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Employee 03", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Employee 04", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Employee 05", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Depto 01",    "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Depto 02",    "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Depto 03",    "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id)
            };

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddAsync(customer);
            await dbContext.AddAsync(department);
            await dbContext.AddRangeAsync(employees);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetEmployeesInput(page, pageSize, departmentId: department.Id, firstName: search);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<EmployeeOutput>>($"/v1/departments/{department.Id}/employees", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (var outputItem in output.Items)
            {
                var employee = employees.FirstOrDefault(x => x.Id == outputItem.Id);
                employee.Should().NotBeNull();
                outputItem.FirstName.Should().Be(employee!.FirstName);
                outputItem.LastName.Should().Be(employee!.LastName);
                outputItem.Document.Should().Be(employee!.Document);
                outputItem.Email.Should().Be(employee!.Email);
                outputItem.IsActive.Should().Be(employee.IsActive);
            }
        }

        [Theory(DisplayName = "Should return filtered data by status")]
        [Trait("E2E/Controllers", "Departments / [v1]GetEmployeesByDepartment")]
        [InlineData(true, 1, 5, 5, 5)]
        [InlineData(false, 1, 5, 3, 3)]
        public async Task SearchByStatus(bool search, int page, int pageSize, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
        {
            var customer = _fixture.GetValidCustomerModel();
            var department = _fixture.GetValidDepartmentModel(customer.Id);

            var employees = new List<EmployeeModel>()
            {
                new EmployeeModel(Guid.NewGuid(), "Employee 01", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Employee 02", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Employee 03", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Employee 04", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Employee 05", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Depto 01",    "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Depto 02",    "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Depto 03",    "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id)
            };

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddAsync(customer);
            await dbContext.AddAsync(department);
            await dbContext.AddRangeAsync(employees);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetEmployeesInput(page, pageSize, departmentId: department.Id, isActive: search);
            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<EmployeeOutput>>($"/v1/departments/{department.Id}/employees", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (var outputItem in output.Items)
            {
                var employee = employees.FirstOrDefault(x => x.Id == outputItem.Id);
                employee.Should().NotBeNull();
                outputItem.FirstName.Should().Be(employee!.FirstName);
                outputItem.LastName.Should().Be(employee!.LastName);
                outputItem.Document.Should().Be(employee!.Document);
                outputItem.Email.Should().Be(employee!.Email);
                outputItem.IsActive.Should().Be(employee.IsActive);
            }
        }

        [Fact(DisplayName = "Should return employees ordered by name")]
        [Trait("E2E/Controllers", "Departments / [v1]GetEmployeesByDepartment")]
        public async Task ListOrdered()
        {
            var customer = _fixture.GetValidCustomerModel();
            var department = _fixture.GetValidDepartmentModel(customer.Id);

            var employees = new List<EmployeeModel>()
            {
                new EmployeeModel(Guid.NewGuid(), "Employee 01", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Employee 02", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Employee 03", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Employee 04", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Employee 05", "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Depto 01",    "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Depto 02",    "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id),
                new EmployeeModel(Guid.NewGuid(), "Depto 03",    "Test",  _fixture.GetValidDocument(), _fixture.GetValidEmployeeEmail(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id, department.Id)
            };

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddAsync(customer);
            await dbContext.AddAsync(department);
            await dbContext.AddRangeAsync(employees);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetEmployeesInput(1, 5, departmentId: department.Id, sort: "firstName");

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<EmployeeOutput>>($"/v1/departments/{department.Id}/employees", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(employees.Count);
            output.Items.Should().HaveCount(5);

            output.Items.ToList()[0].FirstName.Should().Be("Depto 01");
            output.Items.ToList()[1].FirstName.Should().Be("Depto 02");
            output.Items.ToList()[2].FirstName.Should().Be("Depto 03");
            output.Items.ToList()[3].FirstName.Should().Be("Employee 01");
            output.Items.ToList()[4].FirstName.Should().Be("Employee 02");
        }
    }
}
