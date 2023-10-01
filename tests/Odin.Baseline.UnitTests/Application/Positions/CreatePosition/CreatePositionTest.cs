using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Baseline.Application.Positions;
using Odin.Baseline.Application.Positions.CreatePosition;
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
        private readonly Mock<IValidator<CreatePositionInput>> _validatorMock;

        public CreatePositionTest(CreatePositionTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should create a position with valid data")]
        [Trait("Application", "Positions / CreatePosition")]
        public async void CreatePosition()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreatePositionInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var input = _fixture.GetValidCreatePositionInput();
            var positionToInsert = new Position(input.Name, input.BaseSalary);
            var expectedPositionInserted = new PositionOutput
            (
                id: Guid.NewGuid(),
                name: input.Name,
                baseSalary: input.BaseSalary ?? 0,
                isActive: true,
                createdAt: DateTime.UtcNow,
                createdBy: "unit.testing",
                lastUpdatedAt: DateTime.UtcNow,
                lastUpdatedBy: "unit.testing"
            );

            _repositoryMock.Setup(s => s.InsertAsync(It.IsAny<Position>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(positionToInsert));

            var useCase = new App.CreatePosition(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);
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
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Positions / CreatePosition")]
        public async void FluentValidationFailed()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreatePositionInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var input = _fixture.GetValidCreatePositionInput();
            var useCase = new App.CreatePosition(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
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
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreatePositionInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var useCase = new App.CreatePosition(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage(exceptionMessage);
        }
    }
}
