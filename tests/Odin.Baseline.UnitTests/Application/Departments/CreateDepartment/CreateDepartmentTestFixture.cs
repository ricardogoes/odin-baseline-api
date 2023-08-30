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
            => new()
            {
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                LoggedUsername = GetValidUsername()
            };

        public CreateDepartmentInput GetCreateDepartmentInputWithEmptyCustomerId()
            => new()
            {
                CustomerId = Guid.Empty,
                Name = GetValidName(),
                LoggedUsername = GetValidUsername()
            };

        public CreateDepartmentInput GetCreateDepartmentInputWithEmptyName()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                Name = "",
                LoggedUsername = GetValidUsername()
            };

        public CreateDepartmentInput GetCreateDepartmentInputWithEmptyLoggedUsername()
           => new()
           {
               CustomerId = Guid.NewGuid(),
               Name = GetValidName(),
               LoggedUsername = ""
           };
    }
}
