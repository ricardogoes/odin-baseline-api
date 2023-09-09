using FluentAssertions;
using Moq;
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

        public GetPositionsTest(GetPositionsTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should return data filtered")]
        [Trait("Application", "Positions / GetPositions")]
        public async Task GetPositions()
        {
            var expectedPositions = new PaginatedListOutput<Position>
            {
                TotalItems = 4,
                Items = new List<Position>
                {
                    new Position(Guid.NewGuid(), _fixture.GetValidName(), _fixture.GetValidBaseSalary()),
                    new Position(Guid.NewGuid(), _fixture.GetValidName(), _fixture.GetValidBaseSalary()),
                    new Position(Guid.NewGuid(), _fixture.GetValidName(), _fixture.GetValidBaseSalary()),
                    new Position(Guid.NewGuid(), _fixture.GetValidName(), _fixture.GetValidBaseSalary())
                }
            };

            _repositoryMock.Setup(s => s.FindPaginatedListAsync(It.IsAny<Dictionary<string, object>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedPositions));

            var useCase = new App.GetPositions(_repositoryMock.Object);
            var positions = await useCase.Handle(new App.GetPositionsInput(1, 10, "name", Guid.NewGuid(), "Unit Testing", true, "", null, null, "", null, null), new CancellationToken());

            positions.TotalItems.Should().Be(4);
        }
    }
}
