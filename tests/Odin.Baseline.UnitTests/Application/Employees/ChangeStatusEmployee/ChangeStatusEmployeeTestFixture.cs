using Odin.Baseline.Application.Employees.ChangeStatusEmployee;
using Odin.Baseline.UnitTests.Application.Employees.Common;

namespace Odin.Baseline.UnitTests.Application.Employees.ChangeStatusEmployee
{
    [CollectionDefinition(nameof(ChangeStatusEmployeeTestFixtureCollection))]
    public class ChangeStatusEmployeeTestFixtureCollection : ICollectionFixture<ChangeStatusEmployeeTestFixture>
    { }

    public class ChangeStatusEmployeeTestFixture : EmployeeBaseFixture
    {
        public ChangeStatusEmployeeTestFixture()
            : base() { }

        public ChangeStatusEmployeeInput GetValidChangeStatusEmployeeInputToActivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE,
                "unit.testing"
            );

        public ChangeStatusEmployeeInput GetValidChangeStatusEmployeeInputToDeactivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                Baseline.Domain.Enums.ChangeStatusAction.DEACTIVATE,
                "unit.testing"
            );

        public ChangeStatusEmployeeInput GetChangeStatusEmployeeInputWithEmptyAction(Guid? id = null)
          => new
          (
              id ?? Guid.NewGuid(),
              null,
              "unit.testing"
          );

        public ChangeStatusEmployeeInput GetChangeStatusEmployeeInputWithEmptyLoggedUsername(Guid? id = null)
           => new
           (
               id ?? Guid.NewGuid(),
               Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE,
               ""
           );
    }
}
