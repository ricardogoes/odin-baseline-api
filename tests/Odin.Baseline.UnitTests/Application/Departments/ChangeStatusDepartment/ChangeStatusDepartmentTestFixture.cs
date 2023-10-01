using Odin.Baseline.Application.Departments.ChangeStatusDepartment;

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
                Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE
            );

        public ChangeStatusDepartmentInput GetValidChangeStatusDepartmentInputToDeactivate(Guid? id = null)
           => new
           (
               id ?? Guid.NewGuid(),
               Baseline.Domain.Enums.ChangeStatusAction.DEACTIVATE
           );

        public ChangeStatusDepartmentInput GetChangeStatusDepartmentInputWithEmptyAction(Guid? id = null)
          => new
          (
              id ?? Guid.NewGuid(),
              null
          );
    }
}
