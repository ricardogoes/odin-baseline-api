using Odin.Baseline.EndToEndTests.Positions.Common;

namespace Odin.Baseline.EndToEndTests.Positions.GetPositionById
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
