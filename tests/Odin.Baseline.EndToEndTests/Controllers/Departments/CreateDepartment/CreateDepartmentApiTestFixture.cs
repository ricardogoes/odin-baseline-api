using Odin.Baseline.Application.Departments.CreateDepartment;
using Odin.Baseline.EndToEndTests.Controllers.Departments;

namespace Odin.Baseline.EndToEndTests.Controllers.Departments.CreateDepartment
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
            => new(GetValidDepartmentName());

        public CreateDepartmentInput GetInputWithNameEmpty()
            => new("");
    }
}
