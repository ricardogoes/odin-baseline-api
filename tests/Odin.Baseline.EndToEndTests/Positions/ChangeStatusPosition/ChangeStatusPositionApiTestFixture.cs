using Odin.Baseline.Application.Positions.ChangeStatusPosition;
using Odin.Baseline.Domain.Enums;
using Odin.Baseline.EndToEndTests.Positions.Common;

namespace Odin.Baseline.EndToEndTests.Positions.ChangeStatusPosition
{
    [CollectionDefinition(nameof(ChangeStatusPositionApiTestCollection))]
    public class ChangeStatusPositionApiTestCollection : ICollectionFixture<ChangeStatusPositionApiTestFixture>
    { }

    public class ChangeStatusPositionApiTestFixture : PositionBaseFixture
    {
        public ChangeStatusPositionApiTestFixture()
            : base()
        { }

        public ChangeStatusPositionInput GetValidInputToActivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.ACTIVATE,
                GetValidUsername()
            );

        public ChangeStatusPositionInput GetValidInputToDeactivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.DEACTIVATE,
                GetValidUsername()
            );

        public ChangeStatusPositionInput GetInputWithInvalidAction(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.INVALID,
                GetValidUsername()
            );
    }
}
