using Odin.Baseline.Application.Employees.UpdateEmployee;
using Odin.Baseline.EndToEndTests.Employees.Common;

namespace Odin.Baseline.EndToEndTests.Employees.UpdateEmployee
{
    [CollectionDefinition(nameof(UpdateEmployeeApiTestCollection))]
    public class UpdateEmployeeApiTestCollection : ICollectionFixture<UpdateEmployeeApiTestFixture>
    { }

    public class UpdateEmployeeApiTestFixture : EmployeeBaseFixture
    {
        public UpdateEmployeeApiTestFixture()
            : base()
        { }

        public UpdateEmployeeInput GetValidUpdateEmployeeInput(Guid? id = null, Guid? customerId = null, Guid? departmentId = null)
           => new
           (
               id: id ?? Guid.NewGuid(),
               customerId: customerId ?? Guid.NewGuid(),
               departmentId: departmentId,
               firstName: GetValidEmployeeFirstName(),
               lastName: GetValidEmployeeLastName(),
               document: GetValidEmployeeDocument(),
               email: GetValidEmployeeEmail(),
               loggedUsername: "unit.testing"
           );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyFirstName()
            => new
            (
                id: Guid.NewGuid(),
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: "",
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail(),
                loggedUsername: "unit.testing"
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyLastName()
            => new
            (
                id: Guid.NewGuid(),
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: "",
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail(),
                loggedUsername: "unit.testing"
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyDocument()
            => new
            (
                id: Guid.NewGuid(),
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: "",
                email: GetValidEmployeeEmail(),
                loggedUsername: "unit.testing"
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithInvalidDocument()
            => new
            (
                id: Guid.NewGuid(),
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: "12.123.123/0002-10",
                email: GetValidEmployeeEmail(),
                loggedUsername: "unit.testing"
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithInvalidEmail()
            => new
            (
                id: Guid.NewGuid(),
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: "",
                loggedUsername: "unit.testing"
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyLoggedUsername()
           => new
           (
               id: Guid.NewGuid(),
               customerId: Guid.NewGuid(),
               departmentId: Guid.NewGuid(),
               firstName: GetValidEmployeeFirstName(),
               lastName: "12.123.123/0002-10",
               document: GetValidEmployeeDocument(),
               email: GetValidEmployeeEmail(),
               loggedUsername: ""
           );
    }
}
