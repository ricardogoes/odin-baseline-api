using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Baseline.Application.Positions.GetPositionById;
using Odin.Baseline.Application.Positions.GetPositions;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Positions.GetPositionById;

namespace Odin.Baseline.UnitTests.Application.Positions.GetPositionById
{
    [Collection(nameof(GetPositionByIdTestFixture))]
    public class GetPositionByIdTest
    {
        private readonly GetPositionByIdTestFixture _fixture;

        private readonly Mock<IRepository<Position>> _repositoryMock;
        private readonly Mock<IValidator<GetPositionByIdInput>> _validatorMock;

        public GetPositionByIdTest(GetPositionByIdTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = _fixture.GetRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should get a position when searched by valid Id")]
        [Trait("Application", "Positions / GetPositionById")]
        public async Task GetPositionById()
        {
            var validPosition = _fixture.GetValidPosition();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<GetPositionByIdInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validPosition);

            var input = new App.GetPositionByIdInput
            {
                Id = validPosition.Id
            };

            var useCase = new App.GetPositionById(_repositoryMock.Object, _validatorMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            output.Should().NotBeNull();
            output.Name.Should().Be(validPosition.Name);
            output.BaseSalary.Should().Be(validPosition.BaseSalary);
            output.IsActive.Should().Be(validPosition.IsActive);
            output.Id.Should().Be(validPosition.Id);
            output.CreatedAt.Should().Be(validPosition.CreatedAt);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Positions / GetPositionById")]
        public async void FluentValidationFailed()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<GetPositionByIdInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.GetPositionById(_repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(new App.GetPositionByIdInput { Id = Guid.NewGuid() }, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Fact(DisplayName = "Handle() should throw an error when position does not exist")]
        [Trait("Application", "Positions / GetPositionById")]
        public async Task NotFoundExceptionWhenPositionDoesntExist()
        {
            var exampleGuid = Guid.NewGuid();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<GetPositionByIdInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Position '{exampleGuid}' not found"));

            var input = new App.GetPositionByIdInput
            {
                Id = exampleGuid
            };

            var useCase = new App.GetPositionById(_repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();
            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
