using FluentAssertions;
using Moq;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Positions.CreatePosition;

namespace Odin.Baseline.UnitTests.Application.Positions.CreatePosition
{
    [Collection(nameof(CreatePositionTestFixtureCollection))]
    public class CreatePositionTest
    {
        private readonly CreatePositionTestFixture _fixture;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Position>> _repositoryMock;

        public CreatePositionTest(CreatePositionTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should create a position with valid data")]
        [Trait("Application", "Positions / CreatePosition")]
        public async void CreatePosition()
        {
            var input = _fixture.GetValidCreatePositionInput();
            var positionToInsert = new Position(input.CustomerId, input.Name, input.BaseSalary);
            var expectedPositionInserted = new PositionOutput
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                BaseSalary = input.BaseSalary ?? 0,
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow
            };

            _repositoryMock.Setup(s => s.InsertAsync(It.IsAny<Position>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(positionToInsert));

            var useCase = new App.CreatePosition(_unitOfWorkMock.Object, _repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            _unitOfWorkMock.Verify(
                uow => uow.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );

            output.Should().NotBeNull();
            output.Name.Should().Be(expectedPositionInserted.Name);
            output.BaseSalary.Should().Be(expectedPositionInserted.BaseSalary);
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = "Handle() should throw an error when data is invalid")]
        [Trait("Application", "Positions / CreatePosition")]
        [MemberData(
            nameof(CreatePositionTestDataGenerator.GetInvalidInputs),
            parameters: 12,
            MemberType = typeof(CreatePositionTestDataGenerator)
        )]
        public async void ThrowWhenCantInstantiatePosition(
            App.CreatePositionInput input,
            string exceptionMessage
        )
        {
            var useCase = new App.CreatePosition(_unitOfWorkMock.Object, _repositoryMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage(exceptionMessage);
        }
    }
}
