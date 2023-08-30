using Odin.Baseline.Application.Positions.UpdatePosition;
using Odin.Baseline.UnitTests.Application.Positions.Common;

namespace Odin.Baseline.UnitTests.Application.Positions.UpdatePosition
{
    [CollectionDefinition(nameof(UpdatePositionTestFixtureCollection))]
    public class UpdatePositionTestFixtureCollection : ICollectionFixture<UpdatePositionTestFixture>
    { }

    public class UpdatePositionTestFixture : PositionBaseFixture
    {
        public UpdatePositionTestFixture()
            : base() { }

        public UpdatePositionInput GetValidUpdatePositionInput(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                BaseSalary = GetValidBaseSalary(),                
                LoggedUsername = GetValidUsername()
            };

        public UpdatePositionInput GetUpdatePositionInputWithEmptyCustomerId(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                CustomerId = Guid.Empty,
                Name = GetValidName(),
                BaseSalary = GetValidBaseSalary(),
                LoggedUsername = GetValidUsername()
            };

        public UpdatePositionInput GetUpdatePositionInputWithEmptyName(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Name = "",
                BaseSalary = GetValidBaseSalary(),
                LoggedUsername = GetValidUsername()
            };

        public UpdatePositionInput GetUpdatePositionInputWithEmptyLoggedUsername(Guid? id = null)
           => new()
           {
               Id = id ?? Guid.NewGuid(),
               CustomerId = Guid.NewGuid(),
               Name = GetValidName(),
               BaseSalary = GetValidBaseSalary(),
               LoggedUsername = ""
           };
    }


}
