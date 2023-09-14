using Odin.Baseline.Application.Departments.CreateDepartment;
using Odin.Baseline.UnitTests.Application.Departments.Common;

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
            => new
            (
                Guid.NewGuid(),
                GetValidName(),
                GetValidUsername()
            );

        public CreateDepartmentInput GetCreateDepartmentInputWithEmptyCustomerId()
            => new
            (
                Guid.Empty,
                GetValidName(),
                GetValidUsername()
            );

        public CreateDepartmentInput GetCreateDepartmentInputWithEmptyName()
            => new
            (
                Guid.NewGuid(),
                "",
                GetValidUsername()
            );

        public CreateDepartmentInput GetCreateDepartmentInputWithEmptyLoggedUsername()
           => new
           (
               Guid.NewGuid(),
               GetValidName(),
               ""
           );
    }
}
