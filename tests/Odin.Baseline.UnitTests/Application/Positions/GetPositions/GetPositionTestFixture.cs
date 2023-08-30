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
            => new()
            {
                CustomerId = id ?? Guid.NewGuid(),
                Name = Faker.Commerce.Department(),
                IsActive = true
            };
    }
}
