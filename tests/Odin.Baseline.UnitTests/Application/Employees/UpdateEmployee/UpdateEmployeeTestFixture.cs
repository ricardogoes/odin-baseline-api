using Odin.Baseline.Application.Employees.UpdateEmployee;
using Odin.Baseline.UnitTests.Application.Employees.Common;

namespace Odin.Baseline.UnitTests.Application.Employees.UpdateEmployee
{
    [CollectionDefinition(nameof(UpdateEmployeeTestFixtureCollection))]
    public class UpdateEmployeeTestFixtureCollection : ICollectionFixture<UpdateEmployeeTestFixture>
    { }

    public class UpdateEmployeeTestFixture : EmployeeBaseFixture
    {
        public UpdateEmployeeTestFixture()
            : base() { }

        public UpdateEmployeeInput GetValidUpdateEmployeeInput(Guid? id = null)
            => new
            (
                id:  id ?? Guid.NewGuid(),
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: GetValidDocument(),
                email: GetValidEmail(),
                loggedUsername: GetValidUsername()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyFirstName(Guid? id = null)
            => new
            (
                id:  id ?? Guid.NewGuid(),
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: "",
                lastName: GetValidLastName(),
                document: GetValidDocument(),
                email: GetValidEmail(),
                loggedUsername: GetValidUsername()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyLastName(Guid? id = null)
            => new
            (
                id:  id ?? Guid.NewGuid(),
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: "",
                document: GetValidDocument(),
                email: GetValidEmail(),
                loggedUsername: GetValidUsername()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyDocument(Guid? id = null)
            => new
            (
                id: id ?? Guid.NewGuid(),
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: "",
                email: GetValidEmail(),
                loggedUsername: GetValidUsername()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithInvalidDocument(Guid? id = null)
            => new
            (
                id:  id ?? Guid.NewGuid(),
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: GetInvalidDocument(),
                email: GetValidEmail(),
                loggedUsername: GetValidUsername()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithInvalidEmail(Guid? id = null)
            => new
            (
                id:  id ?? Guid.NewGuid(),
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: GetValidDocument(),
                email: "",
                loggedUsername: GetValidUsername()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyLoggedUsername(Guid? id = null)
           => new
           (
               id:  id ?? Guid.NewGuid(),
               customerId: Guid.NewGuid(),
               departmentId: Guid.NewGuid(),
               firstName: GetValidFirstName(),
               lastName: GetInvalidDocument(),
               document: GetValidDocument(),
               email: GetValidEmail(),
               loggedUsername: ""
           );
    }
}
