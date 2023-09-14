using Odin.Baseline.Application.Employees.CreateEmployee;
using Odin.Baseline.EndToEndTests.Employees.Common;

namespace Odin.Baseline.EndToEndTests.Employees.CreateEmployee
{
    [CollectionDefinition(nameof(CreateEmployeeApiTestCollection))]
    public class CreateEmployeeApiTestCollection : ICollectionFixture<CreateEmployeeApiTestFixture>
    { }

    public class CreateEmployeeApiTestFixture : EmployeeBaseFixture
    {
        public CreateEmployeeApiTestFixture()
            : base()
        { }

        public CreateEmployeeInput GetValidCreateEmployeeInput(Guid? customerId = null, Guid? departmentId = null)
            => new
            (
                customerId: customerId ?? Guid.NewGuid(),
                departmentId: departmentId,
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail(),
                loggedUsername: "unit.testing"
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyFirstName()
            => new
            (
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: "",
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail(),
                loggedUsername: "unit.testing"
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyLastName()
            => new
            (
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: "",
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail(),
                loggedUsername: "unit.testing"
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyDocument()
            => new
            (
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: "",
                email: GetValidEmployeeEmail(),
                loggedUsername: "unit.testing"
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithInvalidDocument()
            => new
            (
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: "12.123.123/0002-10",
                email: GetValidEmployeeEmail(),
                loggedUsername: "unit.testing"
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithInvalidEmail()
            => new
            (
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: "",
                loggedUsername: "unit.testing"
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyLoggedUsername()
           => new
           (
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
