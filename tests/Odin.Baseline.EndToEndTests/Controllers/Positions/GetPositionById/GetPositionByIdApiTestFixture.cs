using Odin.Baseline.EndToEndTests.Controllers.Positions;

namespace Odin.Baseline.EndToEndTests.Controllers.Positions.GetPositionById
{
    [CollectionDefinition(nameof(GetPositionByIdApiTestCollection))]
    public class GetPositionByIdApiTestCollection : ICollectionFixture<GetPositionByIdApiTestFixture>
    { }

    public class GetPositionByIdApiTestFixture : PositionBaseFixture
    {
        public GetPositionByIdApiTestFixture()
            : base()
        { }
    }
}
