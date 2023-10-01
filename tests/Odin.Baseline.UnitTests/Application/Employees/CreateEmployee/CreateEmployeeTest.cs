using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Baseline.Application.Employees;
using Odin.Baseline.Application.Employees.CreateEmployee;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.SeedWork;
using App = Odin.Baseline.Application.Employees.CreateEmployee;

namespace Odin.Baseline.UnitTests.Application.Employees.CreateEmployee
{
    [Collection(nameof(CreateEmployeeTestFixtureCollection))]
    public class CreateEmployeeTest
    {
        private readonly CreateEmployeeTestFixture _fixture;

        private readonly Mock<IDocumentService> _documentServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmployeeRepository> _repositoryMock;
        private readonly Mock<IValidator<CreateEmployeeInput>> _validatorMock;

        public CreateEmployeeTest(CreateEmployeeTestFixture fixture)
        {
            _fixture = fixture;

            _documentServiceMock = _fixture.GetDocumentServiceMock();
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should create a employee with valid data")]
        [Trait("Application", "Employees / CreateEmployee")]
        public async void CreateEmployee()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateEmployeeInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var input = _fixture.GetValidCreateEmployeeInput();
            var employeeToInsert = new Employee(input.FirstName, input.LastName, input.Document, input.Email, departmentId: input.DepartmentId, isActive: true);
            var expectedEmployeeInserted = new EmployeeOutput
            (
                id: Guid.NewGuid(),
                firstName: input.FirstName,
                lastName: input.LastName,
                document: input.Document,
                email: input.Email,
                isActive: true,
                createdAt: DateTime.UtcNow,
                createdBy: "unit.testing",
                lastUpdatedAt: DateTime.UtcNow,
                lastUpdatedBy: "unit.testing"
            );

            _documentServiceMock.Setup(s => s.IsDocumentUnique(It.IsAny<EntityWithDocument>(), It.IsAny<CancellationToken>()))
               .Returns(() => Task.FromResult(true));

            _repositoryMock.Setup(s => s.InsertAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(employeeToInsert));

            var useCase = new App.CreateEmployee(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            _unitOfWorkMock.Verify(
                uow => uow.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );

            output.Should().NotBeNull();
            output.FirstName.Should().Be(expectedEmployeeInserted.FirstName);
            output.LastName.Should().Be(expectedEmployeeInserted.LastName);
            output.Document.Should().Be(expectedEmployeeInserted.Document);
            output.Email.Should().Be(expectedEmployeeInserted.Email);
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Employees / CreateEmployee")]
        public async void FluentValidationFailed()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateEmployeeInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var input = _fixture.GetValidCreateEmployeeInput();
            var useCase = new App.CreateEmployee(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Theory(DisplayName = "Handle() should throw an error when data is invalid")]
        [Trait("Application", "Employees / CreateEmployee")]
        [MemberData(
            nameof(CreateEmployeeTestDataGenerator.GetInvalidInputs),
            parameters: 15,
            MemberType = typeof(CreateEmployeeTestDataGenerator)
        )]
        public async void ThrowWhenCantInstantiateEmployee(
            App.CreateEmployeeInput input,
            string exceptionMessage
        )
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateEmployeeInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var useCase = new App.CreateEmployee(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage(exceptionMessage);
        }

        [Fact(DisplayName = "Handle() should throw an error when document already exists")]
        [Trait("Application", "Employees / CreateEmployee")]
        public async void ThrowWhenDocumentAlreadyExists()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateEmployeeInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var input = _fixture.GetValidCreateEmployeeInput();

            _documentServiceMock.Setup(s => s.IsDocumentUnique(It.IsAny<EntityWithDocument>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(false));

            var useCase = new App.CreateEmployee(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage("Document must be unique");
        }
    }
}
