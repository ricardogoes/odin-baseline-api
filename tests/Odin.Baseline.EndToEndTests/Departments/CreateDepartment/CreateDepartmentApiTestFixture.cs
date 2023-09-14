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
            => new
            (
                customerId ?? Guid.NewGuid(),
                GetValidName(),
                GetValidUsername()
            );

        public CreateDepartmentInput GetInputWithCustomerIdEmpty()
            => new
            (
                Guid.Empty,
                GetValidName(),
                GetValidUsername()
            );

        public CreateDepartmentInput GetInputWithNameEmpty()
            => new
            (
                Guid.NewGuid(),
                "",
                GetValidUsername()
            );

        public CreateDepartmentInput GetInputWithUsernameEmpty()
            => new
            (
                Guid.NewGuid(),
                GetValidName(),
                ""
            );
    }
}
