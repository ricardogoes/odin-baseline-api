using FluentAssertions;
using Moq;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Positions.ChangeStatusPosition;

namespace Odin.Baseline.UnitTests.Application.Positions.ChangeStatusPosition
{
    [Collection(nameof(ChangeStatusPositionTestFixtureCollection))]
    public class ChangeStatusPositionTest
    {
        private readonly ChangeStatusPositionTestFixture _fixture;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Position>> _repositoryMock;

        public ChangeStatusPositionTest(ChangeStatusPositionTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should activate a valid position")]
        [Trait("Application", "Positions / ChangeStatusPosition")]
        public async Task HandleShouldActivatePosition()
        {
            var validPosition = _fixture.GetValidPosition();
            var input = _fixture.GetValidChangeStatusPositionInputToActivate();

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validPosition);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Position>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validPosition));

            var useCase = new App.ChangeStatusPosition(_unitOfWorkMock.Object, _repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.IsActive.Should().BeTrue();
            
            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Position>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should deactivate a valid position")]
        [Trait("Application", "Positions / ChangeStatusPosition")]
        public async Task HandleShouldDeactivatePosition()
        {
            var validPosition = _fixture.GetValidPosition();
            var input = _fixture.GetValidChangeStatusPositionInputToDeactivate();

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validPosition);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Position>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validPosition));

            var useCase = new App.ChangeStatusPosition(_unitOfWorkMock.Object, _repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.IsActive.Should().BeFalse();

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Position>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when position not found")]
        [Trait("Application", "Positions / ChangeStatusPosition")]
        public async Task ThrowWhenPositionNotFound()
        {
            var input = _fixture.GetValidChangeStatusPositionInputToActivate();

            _repositoryMock.Setup(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Position '{input.Id}' not found"));

            var useCase = new App.ChangeStatusPosition(_unitOfWorkMock.Object, _repositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            _repositoryMock.Verify(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
