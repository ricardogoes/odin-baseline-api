using Odin.Baseline.Application.Positions.CreatePosition;
using Odin.Baseline.UnitTests.Application.Positions.Common;

namespace Odin.Baseline.UnitTests.Application.Positions.CreatePosition
{
    [CollectionDefinition(nameof(CreatePositionTestFixtureCollection))]
    public class CreatePositionTestFixtureCollection : ICollectionFixture<CreatePositionTestFixture>
    { }

    public class CreatePositionTestFixture : PositionBaseFixture
    {
        public CreatePositionTestFixture()
            : base() { }

        public CreatePositionInput GetValidCreatePositionInput()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                BaseSalary = GetValidBaseSalary(),                
                LoggedUsername = GetValidUsername()
            };

        public CreatePositionInput GetCreatePositionInputWithEmptyCustomerId()
            => new()
            {
                CustomerId = Guid.Empty,
                Name = GetValidName(),
                BaseSalary = GetValidBaseSalary(),
                LoggedUsername = GetValidUsername()
            };

        public CreatePositionInput GetCreatePositionInputWithEmptyName()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                Name = "",
                BaseSalary = GetValidBaseSalary(),
                LoggedUsername = GetValidUsername()
            };

        public CreatePositionInput GetCreatePositionInputWithEmptyLoggedUsername()
           => new()
           {
               CustomerId = Guid.NewGuid(),
               Name = GetValidName(),
               BaseSalary = GetValidBaseSalary(),
               LoggedUsername = ""
           };
    }
}
