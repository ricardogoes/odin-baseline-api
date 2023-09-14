using Odin.Baseline.Application.Employees.AddPosition;
using Odin.Baseline.EndToEndTests.Employees.Common;

namespace Odin.Baseline.EndToEndTests.Employees.AddPosition
{
    [CollectionDefinition(nameof(AddPositionApiTestCollection))]
    public class AddPositionApiTestCollection : ICollectionFixture<AddPositionApiTestFixture>
    { }

    public class AddPositionApiTestFixture : EmployeeBaseFixture
    {
        public AddPositionApiTestFixture()
            : base()
        { }

        public AddPositionInput GetValidInput(Guid? id = null)
        {
            return new AddPositionInput
            (
                employeeId: id ?? Guid.NewGuid(),
                positionId: Guid.NewGuid(),
                salary: 10_000,
                startDate: DateTime.Now,
                finishDate: null,
                loggedUsername: "unit.testing"
            );
        }

        public AddPositionInput GetInputWithEmptyEmployeeId()
        {
            return new AddPositionInput
            (
                employeeId: Guid.Empty,
                positionId: Guid.NewGuid(),
                salary: 10_000,
                startDate: DateTime.Now,
                finishDate: null,
                loggedUsername: "unit.testing"
            );
        }

        public AddPositionInput GetInputWithEmptyPositionId()
        {
            return new AddPositionInput
            (
                employeeId: Guid.NewGuid(),
                positionId: Guid.Empty,
                salary: 10_000,
                startDate: DateTime.Now,
                finishDate: null,
                loggedUsername: "unit.testing"
            );
        }

        public AddPositionInput GetInputWithEmptySalary()
        {
            return new AddPositionInput
            (
                employeeId: Guid.NewGuid(),
                positionId: Guid.NewGuid(),
                salary: 0,
                startDate: DateTime.Now,
                finishDate: null,
                loggedUsername: "unit.testing"
            );
        }

        public AddPositionInput GetInputWithEmptyLoggerUsername()
        {
            return new AddPositionInput
            (
                employeeId: Guid.NewGuid(),
                positionId: Guid.NewGuid(),
                salary: 10_000,
                startDate: DateTime.Now,
                finishDate: null,
                loggedUsername: ""
            );
        }
    }
}
