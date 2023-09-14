using Odin.Baseline.Application.Positions.GetPositions;
using Odin.Baseline.UnitTests.Application.Positions.Common;

namespace Odin.Baseline.UnitTests.Application.Positions.GetPositions
{
    [CollectionDefinition(nameof(GetPositionsTestFixtureCollection))]
    public class GetPositionsTestFixtureCollection : ICollectionFixture<GetPositionsTestFixture>
    { }

    public class GetPositionsTestFixture : PositionBaseFixture
    {
        public GetPositionsInput GetValidGetPositionsInput(Guid? id = null)
            => new GetPositionsInput
            (
                page: 1,
                pageSize: 5,                
                customerId: id ?? Guid.NewGuid(),
                name: Faker.Commerce.Department(),
                isActive: true                
            );
    }
}
