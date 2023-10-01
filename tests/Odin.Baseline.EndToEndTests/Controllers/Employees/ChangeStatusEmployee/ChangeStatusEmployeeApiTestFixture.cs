using Odin.Baseline.Application.Employees.ChangeStatusEmployee;
using Odin.Baseline.Domain.Enums;
using Odin.Baseline.EndToEndTests.Controllers.Employees;

namespace Odin.Baseline.EndToEndTests.Controllers.Employees.ChangeStatusEmployee
{
    [CollectionDefinition(nameof(ChangeStatusEmployeeApiTestCollection))]
    public class ChangeStatusEmployeeApiTestCollection : ICollectionFixture<ChangeStatusEmployeeApiTestFixture>
    { }

    public class ChangeStatusEmployeeApiTestFixture : EmployeeBaseFixture
    {
        public ChangeStatusEmployeeApiTestFixture()
            : base()
        { }

        public ChangeStatusEmployeeInput GetValidInputToActivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.ACTIVATE
            );

        public ChangeStatusEmployeeInput GetValidInputToDeactivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.DEACTIVATE
            );

        public ChangeStatusEmployeeInput GetInputWithInvalidAction(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.INVALID
            );
    }
}
