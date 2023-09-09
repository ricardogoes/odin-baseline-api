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
            => new()
            {
                CustomerId = customerId ?? Guid.NewGuid(),
                Name = GetValidName(),
                LoggedUsername = GetValidUsername()
            };

        public CreatePositionInput GetInputWithCustomerIdEmpty()
            => new()
            {
                CustomerId = Guid.Empty,
                Name = GetValidName(),
                BaseSalary = 1_000,
                LoggedUsername = GetValidUsername()
            };

        public CreatePositionInput GetInputWithNameEmpty()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                Name = "",
                BaseSalary = 1_000,
                LoggedUsername = GetValidUsername()
            };

        public CreatePositionInput GetInputWithUsernameEmpty()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                BaseSalary = 1_000,
                LoggedUsername = ""
            };
    }
}
