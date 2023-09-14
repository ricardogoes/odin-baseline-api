using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Models;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Application.Departments.GetDepartments;
using Odin.Baseline.Infra.Data.EF.Models;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Customers.GetDepartmentsByCustomer
{

    [Collection(nameof(GetDepartmentsByCustomerApiTestCollection))]
    public class GetDepartmentsByCustomerApiTest
    {
        private readonly GetDepartmentsByCustomerApiTestFixture _fixture;

        public GetDepartmentsByCustomerApiTest(GetDepartmentsByCustomerApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should get depertments when searched by a valid customer")]
        [Trait("E2E/Controllers", "Customers / [v1]GetDepartmentsByCustomer")]
        public async Task GetDepartmentsByCustomer()
        {
            var dbContext = _fixture.CreateDbContext(preserveData: false);

            var customersList = _fixture.GetValidCustomersModelList(2);
            var customersIds = customersList.Select(x => x.Id).ToList();

            var departmentsList = _fixture.GetValidDepartmentsModelList(customersIds, 8);   
            
            await dbContext.AddRangeAsync(customersList);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerIdToQuery = customersList.Select(x => x.Id).First();

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<DepartmentOutput>>($"/v1/customers/{customerIdToQuery}/departments");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Should().NotBeNull();
            output.PageNumber.Should().Be(1);
            output.PageSize.Should().Be(5);
            output.TotalRecords.Should().Be(departmentsList.Count/2);
            output.TotalPages.Should().Be(2);
            output.Items.Should().HaveCount(5);
        }

        [Fact(DisplayName = "Should throw an error when customerId is empty")]
        [Trait("E2E/Controllers", "Customers / [v1]GetDepartmentsByCustomer")]
        public async Task ThrowErrorWithEmptyId()
        {
            var (response, output) = await _fixture.ApiClient.GetAsync<ProblemDetails>($"/v1/customers/{Guid.Empty}/departments");

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Fact(DisplayName = "Should return empty list when no data found")]
        [Trait("E2E/Controllers", "Customers / [v1]GetDepartmentsByCustomer")]
        public async Task ItemsEmptyWhenPersistenceEmpty()
        {
            _fixture.CreateDbContext(preserveData: false);
            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<DepartmentOutput>>($"/v1/customers/{Guid.NewGuid()}/departments");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.TotalRecords.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Theory(DisplayName = "Should return paginated data")]
        [Trait("E2E/Controllers", "Customers / [v1]GetDepartmentsByCustomer")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(8, 2, 5, 3)]
        [InlineData(8, 3, 5, 0)]
        public async Task ListPaginated(int quantityToGenerate, int page, int pageSize, int expectedItems)
        {
            var customersList = _fixture.GetValidCustomersModelList(2);
            var customersIds = customersList.Select(x => x.Id).ToList();

            var departmentsList = _fixture.GetValidDepartmentsModelList(customersIds, quantityToGenerate);

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerIdToQuery = customersList.Select(x => x.Id).FirstOrDefault();

            var input = new GetDepartmentsInput(page, pageSize, customerIdToQuery);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<DepartmentOutput>>($"/v1/customers/{customerIdToQuery}/departments", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(quantityToGenerate);
            output.Items.Should().HaveCount(expectedItems);

            foreach (var outputItem in output.Items)
            {
                var department = departmentsList.FirstOrDefault(x => x.Id == outputItem.Id);
                department.Should().NotBeNull();
                outputItem.Name.Should().Be(department!.Name);
                outputItem.IsActive.Should().Be(department.IsActive);
            }
        }

        [Theory(DisplayName = "Should return filtered data by name")]
        [Trait("E2E/Controllers", "Customers / [v1]GetDepartmentsByCustomer")]
        [InlineData("Department", 1, 5, 5, 5)]
        [InlineData("Depto", 1, 5, 3, 3)]
        [InlineData("Invalid", 1, 5, 0, 0)]
        public async Task SearchByName(string search, int page, int pageSize, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
        {
            var dbContext = _fixture.CreateDbContext(preserveData: false);

            var customer = _fixture.GetValidCustomerModel();
            await dbContext.AddAsync(customer);
            
            var departments = new List<DepartmentModel>()
            {
                new DepartmentModel (Guid.NewGuid(), "Department 01", true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Department 02", true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Department 03", true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Department 04", false,DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Department 05", false,DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Depto 01"     , true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Depto 02"     , true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Depto 03"     , false,DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id)
            };

            await dbContext.AddRangeAsync(departments);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetDepartmentsInput(page, pageSize, customer.Id, name: search);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<DepartmentOutput>>($"/v1/customers/{customer.Id}/departments", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (var outputItem in output.Items)
            {
                var department = departments.FirstOrDefault(x => x.Id == outputItem.Id);
                department.Should().NotBeNull();
                outputItem.Name.Should().Be(department!.Name);
                outputItem.IsActive.Should().Be(department.IsActive);
            }
        }

        [Theory(DisplayName = "Should return filtered data by status")]
        [Trait("E2E/Controllers", "Customers / [v1]GetDepartmentsByCustomer")]
        [InlineData(true, 1, 5, 5, 5)]
        [InlineData(false, 1, 5, 3, 3)]
        public async Task SearchByStatus(bool search, int page, int pageSize, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
        {
            var dbContext = _fixture.CreateDbContext(preserveData: false);

            var customer = _fixture.GetValidCustomerModel();
            await dbContext.AddAsync(customer);

            var departments = new List<DepartmentModel>()
            {
                new DepartmentModel (Guid.NewGuid(), "Department 01", true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Department 02", true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Department 03", true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Department 04", false,DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Department 05", false,DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Depto 01"     , true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Depto 02"     , true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Depto 03"     , false,DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id)
            };

            await dbContext.AddRangeAsync(departments);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetDepartmentsInput(page, pageSize, customer.Id, isActive: search);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<DepartmentOutput>>($"/v1/customers/{customer.Id}/departments", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (var outputItem in output.Items)
            {
                var department = departments.FirstOrDefault(x => x.Id == outputItem.Id);
                department.Should().NotBeNull();
                outputItem.Name.Should().Be(department!.Name);
                outputItem.IsActive.Should().Be(department.IsActive);
            }
        }

        [Fact(DisplayName = "Should return departments ordered by name")]
        [Trait("E2E/Controllers", "Customers / [v1]GetDepartmentsByCustomer")]
        public async Task ListOrdered()
        {
            var dbContext = _fixture.CreateDbContext(preserveData: false);
            
            var customer = _fixture.GetValidCustomerModel();
            await dbContext.AddAsync(customer);

            var departments = new List<DepartmentModel>()
            {
                new DepartmentModel (Guid.NewGuid(), "Department 01", true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Department 02", true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Department 03", true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Department 04", false,DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Department 05", false,DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Depto 01"     , true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Depto 02"     , true, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id),
                new DepartmentModel (Guid.NewGuid(), "Depto 03"     , false,DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing", customer.Id)
            };

            await dbContext.AddRangeAsync(departments);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetDepartmentsInput(1, 5, customer.Id, sort: "name");

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<DepartmentOutput>>($"/v1/customers/{customer.Id}/departments", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(departments.Count);
            output.Items.Should().HaveCount(5);

            output.Items.ToList()[0].Name.Should().Be("Department 01");
            output.Items.ToList()[1].Name.Should().Be("Department 02");
            output.Items.ToList()[2].Name.Should().Be("Department 03");
            output.Items.ToList()[3].Name.Should().Be("Department 04");
            output.Items.ToList()[4].Name.Should().Be("Department 05");
        }
    }
}
