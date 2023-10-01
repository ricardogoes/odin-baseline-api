using Odin.Baseline.EndToEndTests.Controllers.Employees;

namespace Odin.Baseline.EndToEndTests.Controllers.Employees.GetEmployeeById
{
    [CollectionDefinition(nameof(GetEmployeeByIdApiTestCollection))]
    public class GetEmployeeByIdApiTestCollection : ICollectionFixture<GetEmployeeByIdApiTestFixture>
    { }

    public class GetEmployeeByIdApiTestFixture : EmployeeBaseFixture
    {
        public GetEmployeeByIdApiTestFixture()
            : base()
        { }
    }
}
