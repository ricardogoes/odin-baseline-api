using Odin.Baseline.Application.Departments.UpdateDepartment;
using Odin.Baseline.UnitTests.Application.Departments.Common;

namespace Odin.Baseline.UnitTests.Application.Departments.UpdateDepartment
{
    [CollectionDefinition(nameof(UpdateDepartmentTestFixtureCollection))]
    public class UpdateDepartmentTestFixtureCollection : ICollectionFixture<UpdateDepartmentTestFixture>
    { }

    public class UpdateDepartmentTestFixture : DepartmentBaseFixture
    {
        public UpdateDepartmentTestFixture()
            : base() { }

        public UpdateDepartmentInput GetValidUpdateDepartmentInput(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                Guid.NewGuid(),
                GetValidName(),             
                GetValidUsername()
            );

        public UpdateDepartmentInput GetUpdateDepartmentInputWithEmptyCustomerId(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                Guid.Empty,
                GetValidName(),
                GetValidUsername()
            );

        public UpdateDepartmentInput GetUpdateDepartmentInputWithEmptyName(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                Guid.NewGuid(),
                "",
                GetValidUsername()
            );

        public UpdateDepartmentInput GetUpdateDepartmentInputWithEmptyLoggedUsername(Guid? id = null)
           => new
           (
               id ?? Guid.NewGuid(),
               Guid.NewGuid(),
               GetValidName(),
               ""
           );
    }


}
