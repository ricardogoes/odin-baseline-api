using Odin.Baseline.EndToEndTests.Controllers.Departments;

namespace Odin.Baseline.EndToEndTests.Controllers.Departments.GetDepartmentById
{
    [CollectionDefinition(nameof(GetDepartmentByIdApiTestCollection))]
    public class GetDepartmentByIdApiTestCollection : ICollectionFixture<GetDepartmentByIdApiTestFixture>
    { }

    public class GetDepartmentByIdApiTestFixture : DepartmentBaseFixture
    {
        public GetDepartmentByIdApiTestFixture()
            : base()
        { }
    }
}
