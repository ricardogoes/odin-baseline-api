using Odin.Baseline.Api.Models.Positions;
using Odin.Baseline.Application.Positions.CreatePosition;
using Odin.Baseline.EndToEndTests.Positions.Common;

namespace Odin.Baseline.EndToEndTests.Positions.CreatePosition
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
            => new
            (
                customerId ?? Guid.NewGuid(),
                GetValidName(),
                10_000,
                GetValidUsername()
            );

        public CreatePositionInput GetInputWithCustomerIdEmpty()
            => new
            (
                Guid.Empty,
                GetValidName(),
                1_000,
                GetValidUsername()
            );

        public CreatePositionInput GetInputWithNameEmpty()
            => new
            (
                Guid.NewGuid(),
                "",
                1_000,
                GetValidUsername()
            );

        public CreatePositionInput GetInputWithUsernameEmpty()
            => new
            (
                Guid.NewGuid(),
                GetValidName(),
                1_000,
                ""
            );
    }
}
