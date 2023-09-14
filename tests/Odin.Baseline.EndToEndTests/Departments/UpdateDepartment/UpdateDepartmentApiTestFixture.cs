using Odin.Baseline.Application.Departments.UpdateDepartment;
using Odin.Baseline.EndToEndTests.Departments.Common;

namespace Odin.Baseline.EndToEndTests.Departments.UpdateDepartment
{
    [CollectionDefinition(nameof(UpdateDepartmentApiTestCollection))]
    public class UpdateDepartmentApiTestCollection : ICollectionFixture<UpdateDepartmentApiTestFixture>
    { }

    public class UpdateDepartmentApiTestFixture : DepartmentBaseFixture
    {
        public UpdateDepartmentApiTestFixture()
            : base()
        { }

        public UpdateDepartmentInput GetValidInput(Guid id, Guid? customerId = null)
            => new
            (
                id: id,
                customerId: customerId ?? Guid.NewGuid(),
                name: GetValidName(),
                loggedUsername:  GetValidUsername()
            );

        public UpdateDepartmentInput GetInputWithIdEmpty()
            => new
            (
                id: Guid.Empty,
                customerId: Guid.NewGuid(),
                name: GetValidName(),
                loggedUsername:  GetValidUsername()
            );


        public UpdateDepartmentInput GetInputWithNameEmpty(Guid id)
            => new
            (
                id: id,
                customerId: Guid.NewGuid(),
                name: "",
                loggedUsername:  GetValidUsername()
            );

        public UpdateDepartmentInput GetInputWithCustomerIdEmpty(Guid id)
            => new
            (
                id: id,
                customerId: Guid.Empty,
                name: "",
                loggedUsername:  GetValidUsername()
            );

        public UpdateDepartmentInput GetInputWithUsernameEmpty(Guid id)
            => new
            (
                id: id,
                customerId: Guid.NewGuid(),
                name: GetValidName(),
                loggedUsername:  ""
            );
    }
}
