using Odin.Baseline.Application.Departments.ChangeStatusDepartment;
using Odin.Baseline.Domain.Enums;
using Odin.Baseline.EndToEndTests.Departments.Common;

namespace Odin.Baseline.EndToEndTests.Departments.ChangeStatusDepartment
{
    [CollectionDefinition(nameof(ChangeStatusDepartmentApiTestCollection))]
    public class ChangeStatusDepartmentApiTestCollection : ICollectionFixture<ChangeStatusDepartmentApiTestFixture>
    { }

    public class ChangeStatusDepartmentApiTestFixture : DepartmentBaseFixture
    {
        public ChangeStatusDepartmentApiTestFixture()
            : base()
        { }

        public ChangeStatusDepartmentInput GetValidInputToActivate(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Action = ChangeStatusAction.ACTIVATE,
                LoggedUsername = GetValidUsername()
            };

        public ChangeStatusDepartmentInput GetValidInputToDeactivate(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Action = ChangeStatusAction.DEACTIVATE,
                LoggedUsername = GetValidUsername()
            };

        public ChangeStatusDepartmentInput GetInputWithInvalidAction(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Action = ChangeStatusAction.INVALID,
                LoggedUsername = GetValidUsername()
            };
    }
}
