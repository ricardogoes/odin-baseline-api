using Odin.Baseline.Application.Positions.CreatePosition;
using Odin.Baseline.EndToEndTests.Controllers.Positions;

namespace Odin.Baseline.EndToEndTests.Controllers.Positions.CreatePosition
{
    [CollectionDefinition(nameof(CreatePositionApiTestCollection))]
    public class CreatePositionApiTestCollection : ICollectionFixture<CreatePositionApiTestFixture>
    { }

    public class CreatePositionApiTestFixture : PositionBaseFixture
    {
        public CreatePositionApiTestFixture()
            : base()
        { }

        public CreatePositionInput GetValidInput(Guid? customerId = null)
            => new(GetValidPositionName(), 10_000);

        public CreatePositionInput GetInputWithNameEmpty()
            => new("", 1_000);
    }
}
