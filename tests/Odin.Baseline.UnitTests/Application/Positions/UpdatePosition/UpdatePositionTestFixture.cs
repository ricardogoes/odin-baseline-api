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
            => new
            (
                id ?? Guid.NewGuid(),
                Guid.NewGuid(),
                GetValidName(),
                GetValidBaseSalary(),                
                GetValidUsername()
            );

        public UpdatePositionInput GetUpdatePositionInputWithEmptyCustomerId(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                Guid.Empty,
                GetValidName(),
                GetValidBaseSalary(),
                GetValidUsername()
            );

        public UpdatePositionInput GetUpdatePositionInputWithEmptyName(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                Guid.NewGuid(),
                "",
                GetValidBaseSalary(),
                GetValidUsername()
            );

        public UpdatePositionInput GetUpdatePositionInputWithEmptyLoggedUsername(Guid? id = null)
           => new
           (
               id ?? Guid.NewGuid(),
               Guid.NewGuid(),
               GetValidName(),
               GetValidBaseSalary(),
               ""
           );
    }


}
