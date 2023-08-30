using Odin.Baseline.EndToEndTests.Departments.Common;

namespace Odin.Baseline.EndToEndTests.Departments.GetDepartmentById
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
