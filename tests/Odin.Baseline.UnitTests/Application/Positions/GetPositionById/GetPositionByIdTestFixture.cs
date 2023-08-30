using Odin.Baseline.Application.Positions.GetPositionById;
using Odin.Baseline.UnitTests.Application.Positions.Common;

namespace Odin.Baseline.UnitTests.Application.Positions.GetPositionById
{
    [CollectionDefinition(nameof(GetPositionByIdTestFixture))]
    public class GetPositionByIdTestFixtureCollection :ICollectionFixture<GetPositionByIdTestFixture>
    { }

    public class GetPositionByIdTestFixture: PositionBaseFixture
    {
        public GetPositionByIdInput GetValidGetPositionByIdInput(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid()
            };
    }
}
