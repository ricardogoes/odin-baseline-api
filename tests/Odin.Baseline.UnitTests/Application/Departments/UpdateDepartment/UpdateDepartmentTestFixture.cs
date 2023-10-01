using Odin.Baseline.Application.Departments.UpdateDepartment;

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
            => new(id ?? Guid.NewGuid(), GetValidName());

        public UpdateDepartmentInput GetUpdateDepartmentInputWithEmptyName(Guid? id = null)
            => new(id ?? Guid.NewGuid(), "");
    }
}
