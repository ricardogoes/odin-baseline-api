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
            var input = new AddPositionInput
            {
                EmployeeId = Guid.NewGuid(),
                PositionId = Guid.NewGuid(),
                Salary = 10_000,
                StartDate = DateTime.Now,
                FinishDate = null,
                LoggedUsername = "unit.testing"
            };
            return input;
        }

        public AddPositionInput GetInputWithEmptyEmployeeId()
        {
            var input = new AddPositionInput
            {
                EmployeeId = Guid.Empty,
                PositionId = Guid.NewGuid(),
                Salary = 10_000,
                StartDate = DateTime.Now,
                FinishDate = null,
                LoggedUsername = "unit.testing"
            };
            return input;
        }

        public AddPositionInput GetInputWithEmptyPositionId()
        {
            var input = new AddPositionInput
            {
                EmployeeId = Guid.NewGuid(),
                PositionId = Guid.Empty,
                Salary = 10_000,
                StartDate = DateTime.Now,
                FinishDate = null,
                LoggedUsername = "unit.testing"
            };
            return input;
        }

        public AddPositionInput GetInputWithEmptySalary()
        {
            var input = new AddPositionInput
            {
                EmployeeId = Guid.NewGuid(),
                PositionId = Guid.NewGuid(),
                Salary = 0,
                StartDate = DateTime.Now,
                FinishDate = null,
                LoggedUsername = "unit.testing"
            };
            return input;
        }

        public AddPositionInput GetInputWithEmptyLoggerUsername()
        {
            var input = new AddPositionInput
            {
                EmployeeId = Guid.NewGuid(),
                PositionId = Guid.NewGuid(),
                Salary = 10_000,
                StartDate = DateTime.Now,
                FinishDate = null,
                LoggedUsername = ""
            };
            return input;
        }
    }
}
