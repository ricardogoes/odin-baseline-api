using Odin.Baseline.Application.Departments.ChangeStatusDepartment;
using Odin.Baseline.UnitTests.Application.Departments.Common;

namespace Odin.Baseline.UnitTests.Application.Departments.ChangeStatusDepartment
{
    [CollectionDefinition(nameof(ChangeStatusDepartmentTestFixtureCollection))]
    public class ChangeStatusDepartmentTestFixtureCollection : ICollectionFixture<ChangeStatusDepartmentTestFixture>
    { }

    public class ChangeStatusDepartmentTestFixture : DepartmentBaseFixture
    {
        public ChangeStatusDepartmentTestFixture()
            : base() { }

        public ChangeStatusDepartmentInput GetValidChangeStatusDepartmentInputToActivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE,
                "unit.testing"
            );

        public ChangeStatusDepartmentInput GetValidChangeStatusDepartmentInputToDeactivate(Guid? id = null)
           => new
           (
               id ?? Guid.NewGuid(),
               Baseline.Domain.Enums.ChangeStatusAction.DEACTIVATE,
               "unit.testing"
           );

        public ChangeStatusDepartmentInput GetChangeStatusDepartmentInputWithEmptyAction(Guid? id = null)
          => new
          (
              id ?? Guid.NewGuid(),
              null,
              "unit.testing"
          );

        public ChangeStatusDepartmentInput GetChangeStatusDepartmentInputWithEmptyLoggedUsername(Guid? id = null)
           => new
           (
               id ?? Guid.NewGuid(),
               Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE,
               ""
           );
    }
}
