using Odin.Baseline.Application.Positions.UpdatePosition;
using Odin.Baseline.EndToEndTests.Positions.Common;

namespace Odin.Baseline.EndToEndTests.Positions.UpdatePosition
{
    [CollectionDefinition(nameof(UpdatePositionApiTestCollection))]
    public class UpdatePositionApiTestCollection : ICollectionFixture<UpdatePositionApiTestFixture>
    { }

    public class UpdatePositionApiTestFixture : PositionBaseFixture
    {
        public UpdatePositionApiTestFixture()
            : base()
        { }

        public UpdatePositionInput GetValidInput(Guid id)
            => new()
            {
                Id = id,
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                BaseSalary = 10_000,
                LoggedUsername = GetValidUsername()
            };

        public UpdatePositionInput GetInputWithIdEmpty()
            => new()
            {
                Id = Guid.Empty,
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                BaseSalary = 10_000,
                LoggedUsername = GetValidUsername()
            };


        public UpdatePositionInput GetInputWithNameEmpty(Guid id)
            => new()
            {
                Id = id,
                CustomerId = Guid.NewGuid(),
                Name = "",
                BaseSalary = 10_000,
                LoggedUsername = GetValidUsername()
            };

        public UpdatePositionInput GetInputWithCustomerIdEmpty(Guid id)
            => new()
            {
                Id = id,
                CustomerId = Guid.Empty,
                Name = "",
                BaseSalary = 10_000,
                LoggedUsername = GetValidUsername()
            };

        public UpdatePositionInput GetInputWithUsernameEmpty(Guid id)
            => new()
            {
                Id = id,
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                BaseSalary = 10_000,
                LoggedUsername = ""
            };
    }
}
