using Odin.Baseline.Application.Employees.ChangeStatusEmployee;

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
                Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE
            );

        public ChangeStatusEmployeeInput GetValidChangeStatusEmployeeInputToDeactivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                Baseline.Domain.Enums.ChangeStatusAction.DEACTIVATE
            );

        public ChangeStatusEmployeeInput GetChangeStatusEmployeeInputWithEmptyAction(Guid? id = null)
          => new
          (
              id ?? Guid.NewGuid(),
              null
          );
    }
}
