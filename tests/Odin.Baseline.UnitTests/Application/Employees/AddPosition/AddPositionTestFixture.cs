using Odin.Baseline.Application.Employees.AddPosition;
using Odin.Baseline.UnitTests.Application.Employees.Common;

namespace Odin.Baseline.UnitTests.Application.Employees.AddPosition
{
    [CollectionDefinition(nameof(AddPositionTestFixtureCollection))]
    public class AddPositionTestFixtureCollection : ICollectionFixture<AddPositionTestFixture>
    { }

    public class AddPositionTestFixture : EmployeeBaseFixture
    {
        public AddPositionTestFixture()
            : base() { }

        public AddPositionInput GetValidInput()
        {
            return new AddPositionInput
            (
                employeeId: Guid.NewGuid(),
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
