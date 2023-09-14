using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Application.Customers.CreateCustomer;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Customers.CreateCustomer
{

    [Collection(nameof(CreateCustomerApiTestCollection))]
    public class CreateCustomerApiTest
    {
        private readonly CreateCustomerApiTestFixture _fixture;

        public CreateCustomerApiTest(CreateCustomerApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should insert a valid customer")]
        [Trait("E2E/Controllers", "Customers / [v1]CreateCustomer")]
        public async Task InsertValidCustomer()
        {
            var input = _fixture.GetValidInput();

            var (response, output) = await _fixture.ApiClient.PostAsync<CustomerOutput>("/v1/customers", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            output.Name.Should().Be(input.Name);
            output.Document.Should().Be(input.Document);
            output.IsActive.Should().BeTrue();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should throw an error when document is duplicated")]
        [Trait("E2E/Controllers", "Customers / [v1]CreateCustomer")]
        public async Task ThrowErrorWithDuplicatedDocument()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = _fixture.GetValidInput();
            input.ChangeDocument(customersList.FirstOrDefault()!.Document);

            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>($"/v1/customers", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            output.Should().NotBeNull();
            output.Title.Should().Be("One or more validation errors ocurred");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
            output.Detail.Should().Be("Document must be unique");
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Customers / [v1]CreateCustomer")]
        [MemberData(
            nameof(CreateCustomerApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(CreateCustomerApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiateCustomer(CreateCustomerInput input, string expectedDetail)
        {
            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>("/v1/customers", input);

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
