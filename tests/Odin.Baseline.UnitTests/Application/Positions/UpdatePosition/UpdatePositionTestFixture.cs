using Odin.Baseline.Application.Positions.UpdatePosition;

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
                GetValidName(),
                GetValidBaseSalary()
            );

        public UpdatePositionInput GetUpdatePositionInputWithEmptyName(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                "",
                GetValidBaseSalary()
            );
    }
}
