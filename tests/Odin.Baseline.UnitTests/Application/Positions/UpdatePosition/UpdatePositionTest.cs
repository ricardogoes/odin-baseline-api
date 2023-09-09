using FluentAssertions;
using Moq;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Positions.UpdatePosition;

namespace Odin.Baseline.UnitTests.Application.Positions.UpdatePosition
{
    [Collection(nameof(UpdatePositionTestFixtureCollection))]
    public class UpdatePositionTest
    {
        private readonly UpdatePositionTestFixture _fixture;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Position>> _repositoryMock;

        public UpdatePositionTest(UpdatePositionTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Theory(DisplayName = "Handle() should update position with valid data")]
        [Trait("Application", "Positions / UpdatePosition")]
        [MemberData(
            nameof(UpdatePositionTestDataGenerator.GetPositionsToUpdate),
            parameters: 10,
            MemberType = typeof(UpdatePositionTestDataGenerator)
        )]
        public async Task UpdatePosition(Position examplePosition, App.UpdatePositionInput input)
        {
            _repositoryMock.Setup(x => x.FindByIdAsync(examplePosition.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(examplePosition);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Position>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(examplePosition));

            var useCase = new App.UpdatePosition(_unitOfWorkMock.Object, _repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.BaseSalary.Should().Be(input.BaseSalary);

            _repositoryMock.Verify(x => x.FindByIdAsync(examplePosition.Id, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Position>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when position not found")]
        [Trait("Application", "Positions / UpdatePosition")]
        public async Task ThrowWhenPositionNotFound()
        {
            var input = _fixture.GetValidUpdatePositionInput();

            _repositoryMock.Setup(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Position '{input.Id}' not found"));

            var useCase = new App.UpdatePosition(_unitOfWorkMock.Object, _repositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            _repositoryMock.Verify(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "Handle() should throw an error when cant update a position")]
        [Trait("Application", "Positions / UpdatePosition")]
        [MemberData(
            nameof(UpdatePositionTestDataGenerator.GetInvalidInputs),
            parameters: 12,
            MemberType = typeof(UpdatePositionTestDataGenerator)
        )]
        public async Task ThrowWhenCantUpdatePosition(App.UpdatePositionInput input, string expectedExceptionMessage)
        {
            var validPosition = _fixture.GetValidPosition();
            input.Id = validPosition.Id;

            _repositoryMock.Setup(x => x.FindByIdAsync(validPosition.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validPosition);

            var useCase = new App.UpdatePosition(_unitOfWorkMock.Object, _repositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>()
                .WithMessage(expectedExceptionMessage);

            _repositoryMock.Verify(x => x.FindByIdAsync(validPosition.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
