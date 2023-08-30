using FluentAssertions;
using Moq;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.SeedWork;
using App = Odin.Baseline.Application.Customers.CreateCustomer;

namespace Odin.Baseline.UnitTests.Application.Customers.CreateCustomer
{
    [Collection(nameof(CreateCustomerTestFixtureCollection))]
    public class CreateCustomerTest
    {
        private readonly CreateCustomerTestFixture _fixture;

        private readonly Mock<IDocumentService> _documentServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICustomerRepository> _repositoryMock;

        public CreateCustomerTest(CreateCustomerTestFixture fixture)
        {
            _fixture = fixture;

            _documentServiceMock = _fixture.GetDocumentServiceMock();
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should create a customer with valid data")]
        [Trait("Application", "Customers / CreateCustomer")]
        public async void CreateCustomer()
        {
            var input = _fixture.GetValidCreateCustomerInput();
            var customerToInsert = new Customer(input.Name, input.Document, isActive: true);
            var expectedCustomerInserted = new CustomerOutput
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                Document = input.Document,
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow
            };

            _documentServiceMock.Setup(s => s.IsDocumentUnique(It.IsAny<EntityWithDocument>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(true));

            _repositoryMock.Setup(s => s.InsertAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(customerToInsert));

            var useCase = new App.CreateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            _unitOfWorkMock.Verify(
                uow => uow.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );

            output.Should().NotBeNull();
            output.Name.Should().Be(expectedCustomerInserted.Name);
            output.Document.Should().Be(expectedCustomerInserted.Document);
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = "Handle() should throw an error when data is invalid")]
        [Trait("Application", "Customers / CreateCustomer")]
        [MemberData(
            nameof(CreateCustomerTestDataGenerator.GetInvalidInputs),
            parameters: 12,
            MemberType = typeof(CreateCustomerTestDataGenerator)
        )]
        public async void ThrowWhenCantInstantiateCustomer(
            App.CreateCustomerInput input,
            string exceptionMessage
        )
        {
            var useCase = new App.CreateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage(exceptionMessage);
        }


        [Fact(DisplayName = "Handle() should throw an error when document already exists")]
        [Trait("Application", "Customers / CreateCustomer")]
        public async void ThrowWhenDocumentAlreadyExists()
        {
            var input = _fixture.GetValidCreateCustomerInput();

            _documentServiceMock.Setup(s => s.IsDocumentUnique(It.IsAny<EntityWithDocument>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(false));

            var useCase = new App.CreateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage("Document must be unique");
        }
    }
}
