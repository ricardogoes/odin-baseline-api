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
            var input = new AddPositionInput
            {
                EmployeeId = id ?? Guid.NewGuid(),
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
