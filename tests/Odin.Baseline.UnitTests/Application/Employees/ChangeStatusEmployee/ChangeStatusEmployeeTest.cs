using FluentAssertions;
using Moq;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Employees.ChangeStatusEmployee;

namespace Odin.Baseline.UnitTests.Application.Employees.ChangeStatusEmployee
{
    [Collection(nameof(ChangeStatusEmployeeTestFixtureCollection))]
    public class ChangeStatusEmployeeTest
    {
        private readonly ChangeStatusEmployeeTestFixture _fixture;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmployeeRepository> _repositoryMock;

        public ChangeStatusEmployeeTest(ChangeStatusEmployeeTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should activate a employee with valid data")]
        [Trait("Application", "Employees / ChangeStatusEmployee")]
        public async Task ActivateEmployee()
        {
            var validEmployee = _fixture.GetValidEmployee();
            var input = _fixture.GetValidChangeStatusEmployeeInputToActivate();

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validEmployee);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validEmployee));

            var useCase = new App.ChangeStatusEmployee(_unitOfWorkMock.Object, _repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.IsActive.Should().BeTrue();
            
            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should deactivate a employee with valid data")]
        [Trait("Application", "Employees / ChangeStatusEmployee")]
        public async Task DeactivateEmployee()
        {
            var validEmployee = _fixture.GetValidEmployee();
            var input = _fixture.GetValidChangeStatusEmployeeInputToDeactivate();

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validEmployee);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validEmployee));

            var useCase = new App.ChangeStatusEmployee(_unitOfWorkMock.Object, _repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.IsActive.Should().BeFalse();

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should change status of a employee with valid data")]
        [Trait("Application", "Employees / ChangeStatusEmployee")]
        public async Task ChangeStatusEmployee()
        {
            var validEmployee = _fixture.GetValidEmployee();
            var input = _fixture.GetValidChangeStatusEmployeeInputToActivate();

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validEmployee);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validEmployee));

            var useCase = new App.ChangeStatusEmployee(_unitOfWorkMock.Object, _repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when employee not found")]
        [Trait("Application", "Employees / ChangeStatusEmployee")]
        public async Task ThrowWhenEmployeeNotFound()
        {
            var input = _fixture.GetValidChangeStatusEmployeeInputToActivate();

            _repositoryMock.Setup(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Employee '{input.Id}' not found"));

            var useCase = new App.ChangeStatusEmployee(_unitOfWorkMock.Object, _repositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            _repositoryMock.Verify(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
