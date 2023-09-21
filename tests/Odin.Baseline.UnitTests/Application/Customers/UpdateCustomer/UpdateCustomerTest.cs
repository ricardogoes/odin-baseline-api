using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Baseline.Application.Customers.UpdateCustomer;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.SeedWork;
using App = Odin.Baseline.Application.Customers.UpdateCustomer;

namespace Odin.Baseline.UnitTests.Application.Customers.UpdateCustomer
{
    [Collection(nameof(UpdateCustomerTestFixtureCollection))]
    public class UpdateCustomerTest
    {
        private readonly UpdateCustomerTestFixture _fixture;

        private readonly Mock<IDocumentService> _documentServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly Mock<IValidator<UpdateCustomerInput>> _validatorMock;

        public UpdateCustomerTest(UpdateCustomerTestFixture fixture)
        {
            _fixture = fixture;

            _documentServiceMock = _fixture.GetDocumentServiceMock();
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
            _validatorMock = new();
        }

        [Theory(DisplayName = "Handle() should update customer with valid data")]
        [Trait("Application", "Customers / UpdateCustomer")]
        [MemberData(
            nameof(UpdateCustomerTestDataGenerator.GetCustomersToUpdate),
            parameters: 10,
            MemberType = typeof(UpdateCustomerTestDataGenerator)
        )]
        public async Task UpdateCustomer(Customer exampleCustomer, App.UpdateCustomerInput input)
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateCustomerInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _documentServiceMock.Setup(s => s.IsDocumentUnique(It.IsAny<EntityWithDocument>(), It.IsAny<CancellationToken>()))
               .Returns(() => Task.FromResult(true));

            _repositoryMock.Setup(x => x.FindByIdAsync(exampleCustomer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(exampleCustomer);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(exampleCustomer));

            var useCase = new App.UpdateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Document.Should().Be(input.Document);

            _repositoryMock.Verify(x => x.FindByIdAsync(exampleCustomer.Id, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Customers / UpdateCustomer")]
        public async void FluentValidationFailed()
        {
            var input = _fixture.GetValidUpdateCustomerInput();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateCustomerInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.UpdateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Fact(DisplayName = "Handle() should throw an error when customer not found")]
        [Trait("Application", "Customers / UpdateCustomer")]
        public async Task ThrowWhenCustomerNotFound()
        {
            var input = _fixture.GetValidUpdateCustomerInput();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateCustomerInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Customer '{input.Id}' not found"));

            var useCase = new App.UpdateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            _repositoryMock.Verify(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "Handle() should throw an error when cant update a customer")]
        [Trait("Application", "Customers / UpdateCustomer")]
        [MemberData(
            nameof(UpdateCustomerTestDataGenerator.GetInvalidInputs),
            parameters: 12,
            MemberType = typeof(UpdateCustomerTestDataGenerator)
        )]
        public async Task ThrowWhenCantUpdateCustomer(App.UpdateCustomerInput input, string expectedExceptionMessage)
        {
            var validCustomer = _fixture.GetValidCustomer();
            input.ChangeId(validCustomer.Id);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateCustomerInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(validCustomer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validCustomer);

            var useCase = new App.UpdateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>()
                .WithMessage(expectedExceptionMessage);

            _repositoryMock.Verify(x => x.FindByIdAsync(validCustomer.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when document already exists")]
        [Trait("Application", "Customers / UpdateCustomer")]
        public async void ThrowWhenDocumentAlreadyExists()
        {
            var input = _fixture.GetValidUpdateCustomerInput();
            var validCustomer = _fixture.GetValidCustomer();
            input.ChangeId(validCustomer.Id);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateCustomerInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(validCustomer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validCustomer);

            _documentServiceMock.Setup(s => s.IsDocumentUnique(It.IsAny<EntityWithDocument>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(false));

            var useCase = new App.UpdateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage("Document must be unique");
        }
    }
}
