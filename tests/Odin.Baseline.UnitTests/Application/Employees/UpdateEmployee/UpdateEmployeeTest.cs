using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Baseline.Application.Employees.UpdateEmployee;
using Odin.Baseline.Application.Positions.UpdatePosition;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.SeedWork;
using App = Odin.Baseline.Application.Employees.UpdateEmployee;

namespace Odin.Baseline.UnitTests.Application.Employees.UpdateEmployee
{
    [Collection(nameof(UpdateEmployeeTestFixtureCollection))]
    public class UpdateEmployeeTest
    {
        private readonly UpdateEmployeeTestFixture _fixture;

        private readonly Mock<IDocumentService> _documentServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmployeeRepository> _repositoryMock;
        private readonly Mock<IValidator<UpdateEmployeeInput>> _validatorMock;

        public UpdateEmployeeTest(UpdateEmployeeTestFixture fixture)
        {
            _fixture = fixture;

            _documentServiceMock = _fixture.GetDocumentServiceMock();
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
            _validatorMock = new();
        }

        [Theory(DisplayName = "Handle() should update employee with valid data")]
        [Trait("Application", "Employees / UpdateEmployee")]
        [MemberData(
            nameof(UpdateEmployeeTestDataGenerator.GetEmployeesToUpdate),
            parameters: 10,
            MemberType = typeof(UpdateEmployeeTestDataGenerator)
        )]
        public async Task UpdateEmployee(Employee exampleEmployee, App.UpdateEmployeeInput input)
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateEmployeeInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _documentServiceMock.Setup(s => s.IsDocumentUnique(It.IsAny<EntityWithDocument>(), It.IsAny<CancellationToken>()))
               .Returns(() => Task.FromResult(true));

            _repositoryMock.Setup(x => x.FindByIdAsync(exampleEmployee.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(exampleEmployee);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(exampleEmployee));

            var useCase = new App.UpdateEmployee(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.FirstName.Should().Be(input.FirstName);
            output.LastName.Should().Be(input.LastName);
            output.Document.Should().Be(input.Document);
            output.Email.Should().Be(input.Email);

            _repositoryMock.Verify(x => x.FindByIdAsync(exampleEmployee.Id, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Employees / UpdateEmployee")]
        public async void FluentValidationFailed()
        {
            var input = _fixture.GetValidUpdateEmployeeInput();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateEmployeeInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.UpdateEmployee(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Fact(DisplayName = "Handle() should throw an error when employee not found")]
        [Trait("Application", "Employees / UpdateEmployee")]
        public async Task ThrowWhenEmployeeNotFound()
        {
            var input = _fixture.GetValidUpdateEmployeeInput();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateEmployeeInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Employee '{input.Id}' not found"));

            var useCase = new App.UpdateEmployee(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            _repositoryMock.Verify(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "Handle() should throw an error when cant update a employee")]
        [Trait("Application", "Employees / UpdateEmployee")]
        [MemberData(
            nameof(UpdateEmployeeTestDataGenerator.GetInvalidInputs),
            parameters: 12,
            MemberType = typeof(UpdateEmployeeTestDataGenerator)
        )]
        public async Task ThrowWhenCantUpdateEmployee(App.UpdateEmployeeInput input, string expectedExceptionMessage)
        {
            var validEmployee = _fixture.GetValidEmployee();
            input.ChangeId(validEmployee.Id);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateEmployeeInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(validEmployee.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validEmployee);

            var useCase = new App.UpdateEmployee(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>()
                .WithMessage(expectedExceptionMessage);

            _repositoryMock.Verify(x => x.FindByIdAsync(validEmployee.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when document already exists")]
        [Trait("Application", "Employees / UpdateEmployee")]
        public async void ThrowWhenDocumentAlreadyExists()
        {
            var input = _fixture.GetValidUpdateEmployeeInput();

            var validEmployee = _fixture.GetValidEmployee();
            input.ChangeId(validEmployee.Id);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateEmployeeInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(validEmployee.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validEmployee);

            _documentServiceMock.Setup(s => s.IsDocumentUnique(It.IsAny<EntityWithDocument>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(false));

            var useCase = new App.UpdateEmployee(_documentServiceMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage("Document must be unique");
        }
    }
}
