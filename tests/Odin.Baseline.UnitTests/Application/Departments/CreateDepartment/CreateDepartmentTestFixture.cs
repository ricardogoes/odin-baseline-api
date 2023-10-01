using Odin.Baseline.Application.Departments.CreateDepartment;

namespace Odin.Baseline.UnitTests.Application.Departments.CreateDepartment
{
    [CollectionDefinition(nameof(CreateDepartmentTestFixtureCollection))]
    public class CreateDepartmentTestFixtureCollection : ICollectionFixture<CreateDepartmentTestFixture>
    { }

    public class CreateDepartmentTestFixture : DepartmentBaseFixture
    {
        public CreateDepartmentTestFixture()
            : base() { }

        public CreateDepartmentInput GetValidCreateDepartmentInput()
            => new(GetValidName());
                    
        public CreateDepartmentInput GetCreateDepartmentInputWithEmptyName()
            => new("");
    }
}
