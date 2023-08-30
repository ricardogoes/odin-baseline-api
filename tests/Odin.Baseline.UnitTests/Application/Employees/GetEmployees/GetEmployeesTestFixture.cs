using Odin.Baseline.UnitTests.Application.Employees.Common;

namespace Odin.Baseline.UnitTests.Application.Employees.GetEmployees
{
    [CollectionDefinition(nameof(GetEmployeesTestFixtureCollection))]
    public class GetEmployeesTestFixtureCollection : ICollectionFixture<GetEmployeesTestFixture>
    { }

    public class GetEmployeesTestFixture : EmployeeBaseFixture
    { }
}
