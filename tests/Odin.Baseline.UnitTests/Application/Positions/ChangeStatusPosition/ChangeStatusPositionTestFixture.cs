using Odin.Baseline.Application.Positions.ChangeStatusPosition;
using Odin.Baseline.UnitTests.Application.Positions.Common;

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
                Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE,
                "unit.testing"
            );

        public ChangeStatusPositionInput GetValidChangeStatusPositionInputToDeactivate(Guid? id = null)
           => new
           (
               id ?? Guid.NewGuid(),
               Baseline.Domain.Enums.ChangeStatusAction.DEACTIVATE,
               "unit.testing"
           );

        public ChangeStatusPositionInput GetChangeStatusPositionInputWithEmptyAction(Guid? id = null)
          => new
          (
              id ?? Guid.NewGuid(),
              null,
              "unit.testing"
          );

        public ChangeStatusPositionInput GetChangeStatusPositionInputWithEmptyLoggedUsername(Guid? id = null)
           => new
           (
               id ?? Guid.NewGuid(),
               Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE,
               ""
           );
    }
}
