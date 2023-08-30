using FluentAssertions;
using Moq;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Customers.GetCustomerById;

namespace Odin.Baseline.UnitTests.Application.Customers.GetCustomerById
{
    [Collection(nameof(GetCustomerByIdTestFixture))]
    public class GetCustomerByIdTest
    {
        private readonly GetCustomerByIdTestFixture _fixture;

        private readonly Mock<ICustomerRepository> _repositoryMock;

        public GetCustomerByIdTest(GetCustomerByIdTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should get a customer when searched by valid Id")]
        [Trait("Application", "Customers / GetCustomerById")]
        public async Task GetCustomerById()
        {
            var validCustomer = _fixture.GetValidCustomer();

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validCustomer);

            var input = new App.GetCustomerByIdInput
            {
                Id = validCustomer.Id
            };

            var useCase = new App.GetCustomerById(_repositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            output.Should().NotBeNull();
            output.Name.Should().Be(validCustomer.Name);
            output.Document.Should().Be(validCustomer.Document);
            output.IsActive.Should().Be(validCustomer.IsActive);
            output.Id.Should().Be(validCustomer.Id);
            output.CreatedAt.Should().Be(validCustomer.CreatedAt);
        }

        [Fact(DisplayName = "Handle() should throw an error when customer does not exist")]
        [Trait("Application", "Customers / GetCustomerById")]
        public async Task NotFoundExceptionWhenCustomerDoesntExist()
        {
            var exampleGuid = Guid.NewGuid();

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Customer '{exampleGuid}' not found"));

            var input = new App.GetCustomerByIdInput
            {
                Id = exampleGuid
            };

            var useCase = new App.GetCustomerById(_repositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();
            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
