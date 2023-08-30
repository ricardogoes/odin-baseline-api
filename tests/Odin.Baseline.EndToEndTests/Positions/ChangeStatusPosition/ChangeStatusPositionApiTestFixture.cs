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
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Action = ChangeStatusAction.ACTIVATE,
                LoggedUsername = GetValidUsername()
            };

        public ChangeStatusPositionInput GetValidInputToDeactivate(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Action = ChangeStatusAction.DEACTIVATE,
                LoggedUsername = GetValidUsername()
            };

        public ChangeStatusPositionInput GetInputWithInvalidAction(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Action = ChangeStatusAction.INVALID,
                LoggedUsername = GetValidUsername()
            };
    }
}
