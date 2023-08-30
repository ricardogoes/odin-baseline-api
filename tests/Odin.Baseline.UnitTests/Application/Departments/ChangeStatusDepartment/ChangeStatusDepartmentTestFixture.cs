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
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Action = Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE,
                LoggedUsername = "unit.testing"
            };

        public ChangeStatusDepartmentInput GetValidChangeStatusDepartmentInputToDeactivate(Guid? id = null)
           => new()
           {
               Id = id ?? Guid.NewGuid(),
               Action = Baseline.Domain.Enums.ChangeStatusAction.DEACTIVATE,
               LoggedUsername = "unit.testing"
           };

        public ChangeStatusDepartmentInput GetChangeStatusDepartmentInputWithEmptyAction(Guid? id = null)
          => new()
          {
              Id = id ?? Guid.NewGuid(),
              Action = null,
              LoggedUsername = "unit.testing"
          };

        public ChangeStatusDepartmentInput GetChangeStatusDepartmentInputWithEmptyLoggedUsername(Guid? id = null)
           => new()
           {
               Id = id ?? Guid.NewGuid(),
               Action = Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE,
               LoggedUsername = ""
           };
    }
}
