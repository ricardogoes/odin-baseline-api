using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Baseline.Application.Employees.AddPosition;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Employees.AddPosition;

namespace Odin.Baseline.UnitTests.Application.Employees.AddPosition
{
    [Collection(nameof(AddPositionTestFixtureCollection))]
    public class AddPositionTest
    {
        private readonly AddPositionTestFixture _fixture;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmployeeRepository> _repositoryMock;
        private readonly Mock<IValidator<AddPositionInput>> _validatorMock;

        public AddPositionTest(AddPositionTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should add a new position with valid data")]
        [Trait("Application", "Employees / AddPosition")]
        public async void AddPosition()
        {
            var input = _fixture.GetValidInput();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<AddPositionInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var positionsHistory = new List<EmployeePositionHistory>
            {
                new EmployeePositionHistory(Guid.NewGuid(), 1_000, DateTime.Now, null, true)
            };

            var employee = _fixture.GetValidEmployee(positionsHistory);
            
            _repositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(employee));

            _repositoryMock.Setup(s => s.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(employee));

            var useCase = new App.AddPosition(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.PositionsHistory.Should().NotBeNull();
            output.PositionsHistory.Should().HaveCount(2);

            var oldPosition = output.PositionsHistory!.First(x => !x.IsActual);
            oldPosition.Should().NotBeNull();
            oldPosition!.FinishDate.Should().NotBeNull();

            var actualPosition = output.PositionsHistory!.First(x => x.IsActual);
            actualPosition.Should().NotBeNull();            
            actualPosition!.FinishDate.Should().BeNull();

            _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()),Times.Once);
            _repositoryMock.Verify(uow => uow.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Employees / AddPosition")]
        public async void FluentValidationFailed()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<AddPositionInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var input = _fixture.GetValidInput();
            var useCase = new App.AddPosition(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Fact(DisplayName = "Handle() should add a new position when list is empty")]
        [Trait("Application", "Employees / AddPosition")]
        public async void AddPositionEmptyList()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<AddPositionInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var input = _fixture.GetValidInput();

            var employee = _fixture.GetValidEmployee();
            
            _repositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(employee));

            _repositoryMock.Setup(s => s.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(employee));

            var useCase = new App.AddPosition(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.PositionsHistory.Should().NotBeNull();
            output.PositionsHistory.Should().HaveCount(1);

            var oldPosition = output.PositionsHistory!.FirstOrDefault(x => !x.IsActual);
            oldPosition.Should().BeNull();

            var actualPosition = output.PositionsHistory!.First(x => x.IsActual);
            actualPosition.Should().NotBeNull();
            actualPosition!.FinishDate.Should().BeNull();

            _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(uow => uow.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
