using Odin.Baseline.EndToEndTests.Employees.Common;

namespace Odin.Baseline.EndToEndTests.Employees.GetEmployeeById
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
