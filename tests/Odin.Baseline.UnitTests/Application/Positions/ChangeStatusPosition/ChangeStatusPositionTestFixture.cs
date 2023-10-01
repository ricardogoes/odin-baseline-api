using Odin.Baseline.Application.Positions.ChangeStatusPosition;

namespace Odin.Baseline.UnitTests.Application.Positions.ChangeStatusPosition
{
    [CollectionDefinition(nameof(ChangeStatusPositionTestFixtureCollection))]
    public class ChangeStatusPositionTestFixtureCollection : ICollectionFixture<ChangeStatusPositionTestFixture>
    { }

    public class ChangeStatusPositionTestFixture : PositionBaseFixture
    {
        public ChangeStatusPositionTestFixture()
            : base() { }

        public ChangeStatusPositionInput GetValidChangeStatusPositionInputToActivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE
            );

        public ChangeStatusPositionInput GetValidChangeStatusPositionInputToDeactivate(Guid? id = null)
           => new
           (
               id ?? Guid.NewGuid(),
               Baseline.Domain.Enums.ChangeStatusAction.DEACTIVATE
           );

        public ChangeStatusPositionInput GetChangeStatusPositionInputWithEmptyAction(Guid? id = null)
          => new
          (
              id ?? Guid.NewGuid(),
              null
          );
    }
}
