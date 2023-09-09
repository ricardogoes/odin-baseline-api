using Odin.Baseline.Application.Departments.CreateDepartment;
using Odin.Baseline.EndToEndTests.Departments.Common;

namespace Odin.Baseline.EndToEndTests.Departments.CreateDepartment
{
    [CollectionDefinition(nameof(CreateDepartmentApiTestCollection))]
    public class CreateDepartmentApiTestCollection : ICollectionFixture<CreateDepartmentApiTestFixture>
    { }

    public class CreateDepartmentApiTestFixture : DepartmentBaseFixture
    {
        public CreateDepartmentApiTestFixture()
            : base()
        { }

        public CreateDepartmentInput GetValidInput(Guid? customerId = null)
            => new()
            {
                CustomerId = customerId ?? Guid.NewGuid(),
                Name = GetValidName(),
                LoggedUsername = GetValidUsername()
            };

        public CreateDepartmentInput GetInputWithCustomerIdEmpty()
            => new()
            {
                CustomerId = Guid.Empty,
                Name = GetValidName(),
                LoggedUsername = GetValidUsername()
            };

        public CreateDepartmentInput GetInputWithNameEmpty()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                Name = "",
                LoggedUsername = GetValidUsername()
            };

        public CreateDepartmentInput GetInputWithUsernameEmpty()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                LoggedUsername = ""
            };
    }
}
