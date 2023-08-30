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
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),             
                LoggedUsername = GetValidUsername()
            };

        public UpdateDepartmentInput GetUpdateDepartmentInputWithEmptyCustomerId(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                CustomerId = Guid.Empty,
                Name = GetValidName(),
                LoggedUsername = GetValidUsername()
            };

        public UpdateDepartmentInput GetUpdateDepartmentInputWithEmptyName(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Name = "",
                LoggedUsername = GetValidUsername()
            };

        public UpdateDepartmentInput GetUpdateDepartmentInputWithEmptyLoggedUsername(Guid? id = null)
           => new()
           {
               Id = id ?? Guid.NewGuid(),
               CustomerId = Guid.NewGuid(),
               Name = GetValidName(),
               LoggedUsername = ""
           };
    }


}
