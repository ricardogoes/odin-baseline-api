using Odin.Baseline.Application.Departments.ChangeStatusDepartment;
using Odin.Baseline.Domain.Enums;
using Odin.Baseline.EndToEndTests.Controllers.Departments;

namespace Odin.Baseline.EndToEndTests.Controllers.Departments.ChangeStatusDepartment
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
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.ACTIVATE
            );

        public ChangeStatusDepartmentInput GetValidInputToDeactivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.DEACTIVATE
            );

        public ChangeStatusDepartmentInput GetInputWithInvalidAction(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.INVALID
            );
    }
}
