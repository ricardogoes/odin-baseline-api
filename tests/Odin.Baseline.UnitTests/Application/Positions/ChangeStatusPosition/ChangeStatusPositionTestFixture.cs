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
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Action = Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE,
                LoggedUsername = "unit.testing"
            };

        public ChangeStatusPositionInput GetValidChangeStatusPositionInputToDeactivate(Guid? id = null)
           => new()
           {
               Id = id ?? Guid.NewGuid(),
               Action = Baseline.Domain.Enums.ChangeStatusAction.DEACTIVATE,
               LoggedUsername = "unit.testing"
           };

        public ChangeStatusPositionInput GetChangeStatusPositionInputWithEmptyAction(Guid? id = null)
          => new()
          {
              Id = id ?? Guid.NewGuid(),
              Action = null,
              LoggedUsername = "unit.testing"
          };

        public ChangeStatusPositionInput GetChangeStatusPositionInputWithEmptyLoggedUsername(Guid? id = null)
           => new()
           {
               Id = id ?? Guid.NewGuid(),
               Action = Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE,
               LoggedUsername = ""
           };
    }
}
