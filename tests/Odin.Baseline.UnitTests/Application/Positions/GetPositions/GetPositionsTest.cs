using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Baseline.Application.Positions.GetPositions;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Models;
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
                    new Position(_fixture.GetValidName(), _fixture.GetValidBaseSalary()),
                    new Position(_fixture.GetValidName(), _fixture.GetValidBaseSalary()),
                    new Position(_fixture.GetValidName(), _fixture.GetValidBaseSalary()),
                    new Position(_fixture.GetValidName(), _fixture.GetValidBaseSalary())
                }
            );

            _repositoryMock.Setup(s => s.FindPaginatedListAsync(It.IsAny<Dictionary<string, object?>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedPositions));

            var useCase = new App.GetPositions(_repositoryMock.Object, _validatorMock.Object);
            var positions = await useCase.Handle(new App.GetPositionsInput(1, 10, "name", "Unit Testing", true, "", null, null, "", null, null), new CancellationToken());

            positions.TotalItems.Should().Be(4);
        }
    }
}
