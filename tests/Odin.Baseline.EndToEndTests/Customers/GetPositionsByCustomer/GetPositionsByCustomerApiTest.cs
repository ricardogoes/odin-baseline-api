using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Models;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Application.Positions.GetPositions;
using Odin.Baseline.Infra.Data.EF.Models;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Customers.GetPositionsByCustomer
{

    [Collection(nameof(GetPositionsByCustomerApiTestCollection))]
    public class GetPositionsByCustomerApiTest
    {
        private readonly GetPositionsByCustomerApiTestFixture _fixture;

        public GetPositionsByCustomerApiTest(GetPositionsByCustomerApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should get positions when searched by a valid customer")]
        [Trait("E2E/Controllers", "Customers / [v1]GetPositionsByCustomer")]
        public async Task GetPositionsByCustomer()
        {
            var customersList = _fixture.GetValidCustomersModelList(2);
            var customersIds = customersList.Select(x => x.Id).ToList();

            var positionsList = _fixture.GetValidPositionsModelList(customersIds, 8);                        

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.AddRangeAsync(positionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerIdToQuery = customersList.Select(x => x.Id).FirstOrDefault();

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<PositionOutput>>($"/v1/customers/{customerIdToQuery}/positions");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Should().NotBeNull();
            output.PageNumber.Should().Be(1);
            output.PageSize.Should().Be(5);
            output.TotalRecords.Should().Be(positionsList.Count/2);
            output.TotalPages.Should().Be(2);
            output.Items.Should().HaveCount(5);
        }

        [Fact(DisplayName = "Should throw an error when customerId is empty")]
        [Trait("E2E/Controllers", "Customers / [v1]GetPositionsByCustomer")]
        public async Task ThrowErrorWithEmptyId()
        {
            var (response, output) = await _fixture.ApiClient.GetAsync<ProblemDetails>($"/v1/customers/{Guid.Empty}/positions");

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Fact(DisplayName = "Should return empty list when no data found")]
        [Trait("E2E/Controllers", "Customers / [v1]GetPositionsByCustomer")]
        public async Task ItemsEmptyWhenPersistenceEmpty()
        {
            _fixture.CreateDbContext(preserveData: false);
            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<PositionOutput>>($"/v1/customers/{Guid.NewGuid()}/positions");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.TotalRecords.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Theory(DisplayName = "Should return paginated data")]
        [Trait("E2E/Controllers", "Customers / [v1]GetPositionsByCustomer")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(8, 2, 5, 3)]
        [InlineData(8, 3, 5, 0)]
        public async Task ListPaginated(int quantityToGenerate, int page, int pageSize, int expectedItems)
        {
            var customersList = _fixture.GetValidCustomersModelList(2);
            var customersIds = customersList.Select(x => x.Id).ToList();

            var positionsList = _fixture.GetValidPositionsModelList(customersIds, quantityToGenerate);

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.AddRangeAsync(positionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerIdToQuery = customersList.Select(x => x.Id).FirstOrDefault();

            var input = new GetPositionsInput
            {
                PageNumber = page,
                PageSize = pageSize
            };

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<PositionOutput>>($"/v1/customers/{customerIdToQuery}/positions", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(quantityToGenerate);
            output.Items.Should().HaveCount(expectedItems);

            foreach (var outputItem in output.Items)
            {
                var position = positionsList.FirstOrDefault(x => x.Id == outputItem.Id);
                position.Should().NotBeNull();
                outputItem.Name.Should().Be(position!.Name);
                outputItem.IsActive.Should().Be(position.IsActive);
            }
        }

        [Theory(DisplayName = "Should return filtered data by name")]
        [Trait("E2E/Controllers", "Customers / [v1]GetPositionsByCustomer")]
        [InlineData("Position", 1, 5, 5, 5)]
        [InlineData("Depto", 1, 5, 3, 3)]
        [InlineData("Invalid", 1, 5, 0, 0)]
        public async Task SearchByName(string search, int page, int pageSize, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
        {
            var customer = _fixture.GetValidCustomerModel();
            
            var positions = new List<PositionModel>()
            {
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 01", BaseSalary = 10_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 02", BaseSalary = 10_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 03", BaseSalary = 10_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 04", BaseSalary = 10_000, IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 05", BaseSalary = 10_000, IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Depto 01", BaseSalary = 5_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Depto 02", BaseSalary = 5_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Depto 03", BaseSalary = 5_000, IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"}
            };

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddAsync(customer);
            await dbContext.AddRangeAsync(positions);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetPositionsInput
            {
                PageNumber = page,
                PageSize = pageSize,
                Name = search
            };

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<PositionOutput>>($"/v1/customers/{customer.Id}/positions", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (var outputItem in output.Items)
            {
                var position = positions.FirstOrDefault(x => x.Id == outputItem.Id);
                position.Should().NotBeNull();
                outputItem.Name.Should().Be(position!.Name);
                outputItem.IsActive.Should().Be(position.IsActive);
            }
        }

        [Theory(DisplayName = "Should return filtered data by status")]
        [Trait("E2E/Controllers", "Customers / [v1]GetPositionsByCustomer")]
        [InlineData(true, 1, 5, 5, 5)]
        [InlineData(false, 1, 5, 3, 3)]
        public async Task SearchByStatus(bool search, int page, int pageSize, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
        {
            var customer = _fixture.GetValidCustomerModel();

            var positions = new List<PositionModel>()
            {
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 01", BaseSalary = 10_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 02", BaseSalary = 10_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 03", BaseSalary = 10_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 04", BaseSalary = 10_000, IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 05", BaseSalary = 10_000, IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Depto 01", BaseSalary = 5_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Depto 02", BaseSalary = 5_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Depto 03", BaseSalary = 5_000, IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"}
            };

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddAsync(customer);
            await dbContext.AddRangeAsync(positions);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetPositionsInput
            {
                PageNumber = page,
                PageSize = pageSize,
                IsActive = search
            };

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<PositionOutput>>($"/v1/customers/{customer.Id}/positions", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (var outputItem in output.Items)
            {
                var position = positions.FirstOrDefault(x => x.Id == outputItem.Id);
                position.Should().NotBeNull();
                outputItem.Name.Should().Be(position!.Name);
                outputItem.IsActive.Should().Be(position.IsActive);
            }
        }

        [Fact(DisplayName = "Should return positions ordered by name")]
        [Trait("E2E/Controllers", "Customers / [v1]GetPositionsByCustomer")]
        public async Task ListOrdered()
        {
            var customer = _fixture.GetValidCustomerModel();
            var positions = new List<PositionModel>()
            {
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 04", BaseSalary = 10_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 05", BaseSalary = 10_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 03", BaseSalary = 10_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 01", BaseSalary = 10_000, IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Position 02", BaseSalary = 10_000, IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Pstion 11", BaseSalary = 5_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Pstion 12", BaseSalary = 5_000, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"},
                new PositionModel { Id = Guid.NewGuid(), CustomerId = customer.Id, Name = "Pstion 23", BaseSalary = 5_000, IsActive = false, CreatedAt = DateTime.Now, CreatedBy = "unit.Testing", LastUpdatedAt = DateTime.Now, LastUpdatedBy = "unit.testing"}
            };

            var dbContext = _fixture.CreateDbContext(preserveData: false);
            await dbContext.AddAsync(customer);
            await dbContext.AddRangeAsync(positions);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetPositionsInput
            {
                PageNumber = 1,
                PageSize = 5,
                Sort = "name"
            };

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<PositionOutput>>($"/v1/customers/{customer.Id}/positions", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(positions.Count);
            output.Items.Should().HaveCount(5);

            output.Items.ToList()[0].Name.Should().Be("Position 01");
            output.Items.ToList()[1].Name.Should().Be("Position 02");
            output.Items.ToList()[2].Name.Should().Be("Position 03");
            output.Items.ToList()[3].Name.Should().Be("Position 04");
            output.Items.ToList()[4].Name.Should().Be("Position 05");
        }
    }
}
