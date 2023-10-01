using Odin.Baseline.Application.Positions.GetPositions;

namespace Odin.Baseline.UnitTests.Application.Positions.GetPositions
{
    [CollectionDefinition(nameof(GetPositionsTestFixtureCollection))]
    public class GetPositionsTestFixtureCollection : ICollectionFixture<GetPositionsTestFixture>
    { }

    public class GetPositionsTestFixture : PositionBaseFixture
    {
        public GetPositionsInput GetValidGetPositionsInput()
            => new GetPositionsInput
            (
                page: 1,
                pageSize: 5,                
                name: Faker.Commerce.Department(),
                isActive: true                
            );
    }
}
