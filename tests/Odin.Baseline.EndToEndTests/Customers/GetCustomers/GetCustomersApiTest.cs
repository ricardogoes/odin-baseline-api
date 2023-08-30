using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Odin.Baseline.Api.Models;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Application.Customers.GetCustomers;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Customers.GetCustomers
{

    [Collection(nameof(GetCustomersApiTestCollection))]
    public class GetCustomersApiTest
    {
        private readonly GetCustomersApiTestFixture _fixture;

        public GetCustomersApiTest(GetCustomersApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should get customers with default data")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        public async Task GetCustomers()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Should().NotBeNull();
            output.PageNumber.Should().Be(1);
            output.PageSize.Should().Be(10);
            output.TotalRecords.Should().Be(20);
            output.TotalPages.Should().Be(2);
            output.Items.Should().HaveCount(10);
        }



        [Fact(DisplayName = "Should return empty list when no data found")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        public async Task ItemsEmptyWhenPersistenceEmpty()
        {
            var dbContext = _fixture.CreateDbContext(preserveData: false);
            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.TotalRecords.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Should return valid data")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        public async Task ListCategoriesAndTotal()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetCustomersInput
            {
                PageNumber = 1,
                PageSize = 5
            };

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Should().NotBeNull();
            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(customersList.Count);
            output.Items.Should().HaveCount(input.PageSize);

            foreach (var outputItem in output.Items)
            {
                var customer = customersList.FirstOrDefault(x => x.Id == outputItem.Id);
                customer.Should().NotBeNull();
                outputItem.Name.Should().Be(customer!.Name);
                outputItem.Document.Should().Be(customer.Document);
                outputItem.IsActive.Should().Be(customer.IsActive);
            }
        }

        [Theory(DisplayName = "Should return paginated data")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        public async Task ListPaginated(int quantityToGenerate, int page, int pageSize, int expectedItems)
        {
            var customersList = _fixture.GetValidCustomersModelList(quantityToGenerate);

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetCustomersInput
            {
                PageNumber = page,
                PageSize = pageSize
            };

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(quantityToGenerate);
            output.Items.Should().HaveCount(expectedItems);

            foreach (var outputItem in output.Items)
            {
                var customer = customersList.FirstOrDefault(x => x.Id == outputItem.Id);
                customer.Should().NotBeNull();
                outputItem.Name.Should().Be(customer!.Name);
                outputItem.Document.Should().Be(customer.Document);
                outputItem.IsActive.Should().Be(customer.IsActive);
            }
        }

        [Theory(DisplayName = "Should return filtered data by name")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        [InlineData("Customer", 1, 5, 5, 5)]
        [InlineData("Cliente", 1, 5, 3, 3)]
        [InlineData("Invalid", 1, 5, 0, 0)]
        public async Task SearchByName(string search, int page, int pageSize, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
        {
            var customersList = new List<CustomerModel>()
            {
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 01", Document = _fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 02", Document = _fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 03", Document = _fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 04", Document = _fixture.GetValidDocument(), IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 05", Document = _fixture.GetValidDocument(), IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Cliente 01",  Document =_fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Cliente 02",  Document =_fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Cliente 03",  Document =_fixture.GetValidDocument(), IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"}
            };

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetCustomersInput
            {
                PageNumber = page,
                PageSize = pageSize,
                Name = search
            };

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (var outputItem in output.Items)
            {
                var customer = customersList.FirstOrDefault(x => x.Id == outputItem.Id);
                customer.Should().NotBeNull();
                outputItem.Name.Should().Be(customer!.Name);
                outputItem.Document.Should().Be(customer.Document);
                outputItem.IsActive.Should().Be(customer.IsActive);
            }
        }

        [Fact(DisplayName = "Should return filtered data by document")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        public async Task SearchByDocument()
        {
            var customersList = new List<CustomerModel>()
            {
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 01", Document = _fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 02", Document = _fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 03", Document = _fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 04", Document = _fixture.GetValidDocument(), IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 05", Document = _fixture.GetValidDocument(), IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Cliente 01",  Document =_fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Cliente 02",  Document =_fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Cliente 03",  Document =_fixture.GetValidDocument(), IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"}
            };

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetCustomersInput
            {
                PageNumber = 1,
                PageSize = 5,
                Document = customersList.Where(x => x.Name == "Cliente 01").FirstOrDefault().Document
            };

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(1);
            output.Items.Should().HaveCount(1);

            foreach (var outputItem in output.Items)
            {
                var customer = customersList.FirstOrDefault(x => x.Id == outputItem.Id);
                customer.Should().NotBeNull();
                outputItem.Name.Should().Be(customer!.Name);
                outputItem.Document.Should().Be(customer.Document);
                outputItem.IsActive.Should().Be(customer.IsActive);
            }
        }

        [Theory(DisplayName = "Should return filtered data by status")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        [InlineData(true, 1, 5, 5, 5)]
        [InlineData(false, 1, 5, 3, 3)]
        public async Task SearchByStatus(bool search, int page, int pageSize, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
        {
            var customersList = new List<CustomerModel>()
            {
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 01", Document = _fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 02", Document = _fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 03", Document = _fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 04", Document = _fixture.GetValidDocument(), IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Customer 05", Document = _fixture.GetValidDocument(), IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Cliente 01",  Document =_fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Cliente 02",  Document =_fixture.GetValidDocument(), IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new CustomerModel { Id = Guid.NewGuid(), Name = "Cliente 03",  Document =_fixture.GetValidDocument(), IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"}
            };

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetCustomersInput
            {
                PageNumber = page,
                PageSize = pageSize,
                IsActive = search
            };

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (var outputItem in output.Items)
            {
                var customer = customersList.FirstOrDefault(x => x.Id == outputItem.Id);
                customer.Should().NotBeNull();
                outputItem.Name.Should().Be(customer!.Name);
                outputItem.Document.Should().Be(customer.Document);
                outputItem.IsActive.Should().Be(customer.IsActive);
            }
        }

        [Theory(DisplayName = "Should return customers ordered by field")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        [InlineData("name")]
        [InlineData("name desc")]
        [InlineData("id")]
        [InlineData("id desc")]
        public async Task ListOrdered(string orderBy)
        {
            var customersList = _fixture.GetValidCustomersModelList(10);

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetCustomersInput
            {
                PageNumber = 1,
                PageSize = 5,
                Sort = orderBy
            };

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(customersList.Count);
            output.Items.Should().HaveCount(5);
        }
    }
}
