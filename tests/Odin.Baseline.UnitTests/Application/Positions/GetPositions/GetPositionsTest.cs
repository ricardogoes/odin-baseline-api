using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Baseline.Application.Customers.UpdateCustomer;
using Odin.Baseline.Application.Positions.GetPositions;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Positions.GetPositions;

namespace Odin.Baseline.UnitTests.Application.Positions.GetPositions
{
    [Collection(nameof(GetPositionsTestFixtureCollection))]
    public class GetPositionsTest
    {
        private readonly GetPositionsTestFixture _fixture;

        private readonly Mock<IRepository<Position>> _repositoryMock;
        private readonly Mock<IValidator<GetPositionsInput>> _validatorMock;

        public GetPositionsTest(GetPositionsTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = _fixture.GetRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should return data filtered")]
        [Trait("Application", "Positions / GetPositions")]
        public async Task GetPositions()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<GetPositionsInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
                        
            var expectedPositions = new PaginatedListOutput<Position>
            (
                totalItems: 4,
                items: new List<Position>
                {
                    new Position(Guid.NewGuid(), _fixture.GetValidName(), _fixture.GetValidBaseSalary()),
                    new Position(Guid.NewGuid(), _fixture.GetValidName(), _fixture.GetValidBaseSalary()),
                    new Position(Guid.NewGuid(), _fixture.GetValidName(), _fixture.GetValidBaseSalary()),
                    new Position(Guid.NewGuid(), _fixture.GetValidName(), _fixture.GetValidBaseSalary())
                }
            );

            _repositoryMock.Setup(s => s.FindPaginatedListAsync(It.IsAny<Dictionary<string, object?>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedPositions));

            var useCase = new App.GetPositions(_repositoryMock.Object, _validatorMock.Object);
            var positions = await useCase.Handle(new App.GetPositionsInput(1, 10, Guid.NewGuid(), "name", "Unit Testing", true, "", null, null, "", null, null), new CancellationToken());

            positions.TotalItems.Should().Be(4);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Positions / GetPositions")]
        public async void FluentValidationFailed()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<GetPositionsInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.GetPositions(_repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(new App.GetPositionsInput(1, 10, Guid.NewGuid(), "name", "Unit Testing", true, "", null, null, "", null, null), CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }
    }
}
