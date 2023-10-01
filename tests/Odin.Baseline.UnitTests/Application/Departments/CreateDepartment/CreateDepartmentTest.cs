using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Baseline.Application.Departments;
using Odin.Baseline.Application.Departments.CreateDepartment;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Departments.CreateDepartment;

namespace Odin.Baseline.UnitTests.Application.Departments.CreateDepartment
{
    [Collection(nameof(CreateDepartmentTestFixtureCollection))]
    public class CreateDepartmentTest
    {
        private readonly CreateDepartmentTestFixture _fixture;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Department>> _repositoryMock;
        private readonly Mock<IValidator<CreateDepartmentInput>> _validatorMock;

        public CreateDepartmentTest(CreateDepartmentTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should create a department with valid data")]
        [Trait("Application", "Departments / CreateDepartment")]
        public async void CreateDepartment()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateDepartmentInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var input = _fixture.GetValidCreateDepartmentInput();
            var departmentToInsert = new Department(input.Name);
            var expectedDepartmentInserted = new DepartmentOutput
            (
                Guid.NewGuid(),
                input.Name,
                true,
                DateTime.UtcNow,
                "unit.testing",
                DateTime.UtcNow,
                "unit.testing"
            );

            _repositoryMock.Setup(s => s.InsertAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(departmentToInsert));

            var useCase = new App.CreateDepartment(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            _unitOfWorkMock.Verify(
                uow => uow.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );

            output.Should().NotBeNull();
            output.Name.Should().Be(expectedDepartmentInserted.Name);
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Departments / CreateDepartment")]
        public async void FluentValidationFailed()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateDepartmentInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var input = _fixture.GetValidCreateDepartmentInput();
            var useCase = new App.CreateDepartment(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Theory(DisplayName = "Handle() should throw an error when name is invalid")]
        [Trait("Application", "Departments / CreateDepartment")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void ThrowWhenCantInstantiateDepartment(string name)
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateDepartmentInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var useCase = new App.CreateDepartment(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            Func<Task> task = async () => await useCase.Handle(new CreateDepartmentInput(name!), CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }
    }
}
