using FluentAssertions;
using Moq;
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

        public AddPositionTest(AddPositionTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should add a new position with valid data")]
        [Trait("Application", "Employees / AddPosition")]
        public async void AddPosition()
        {
            var input = _fixture.GetValidInput();

            var positionsHistory = new List<EmployeePositionHistory>
            {
                new EmployeePositionHistory(Guid.NewGuid(), 1_000, DateTime.Now, null, true)
            };

            var employee = _fixture.GetValidEmployee(positionsHistory);
            
            _repositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(employee));

            _repositoryMock.Setup(s => s.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(employee));

            var useCase = new App.AddPosition(_unitOfWorkMock.Object, _repositoryMock.Object);
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

        [Fact(DisplayName = "Handle() should add a new position when list is empty")]
        [Trait("Application", "Employees / AddPosition")]
        public async void AddPositionEmptyList()
        {
            var input = _fixture.GetValidInput();

            var employee = _fixture.GetValidEmployee();
            
            _repositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(employee));

            _repositoryMock.Setup(s => s.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(employee));

            var useCase = new App.AddPosition(_unitOfWorkMock.Object, _repositoryMock.Object);
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
