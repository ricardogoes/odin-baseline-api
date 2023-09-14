using Odin.Baseline.Application.Employees.CreateEmployee;
using Odin.Baseline.UnitTests.Application.Employees.Common;

namespace Odin.Baseline.UnitTests.Application.Employees.CreateEmployee
{
    [CollectionDefinition(nameof(CreateEmployeeTestFixtureCollection))]
    public class CreateEmployeeTestFixtureCollection : ICollectionFixture<CreateEmployeeTestFixture>
    { }

    public class CreateEmployeeTestFixture : EmployeeBaseFixture
    {
        public CreateEmployeeTestFixture()
            : base() { }

        public CreateEmployeeInput GetValidCreateEmployeeInput()
            => new
            (
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: GetValidDocument(),
                email: GetValidEmail(),
                loggedUsername: GetValidUsername()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyFirstName()
            => new
            (
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: "",
                lastName: GetValidLastName(),
                document: GetValidDocument(),
                email: GetValidEmail(),
                loggedUsername: GetValidUsername()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyLastName()
            => new
            (
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: "",
                document: GetValidDocument(),
                email: GetValidEmail(),
                loggedUsername: GetValidUsername()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyDocument()
            => new
            (
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: "",
                email: GetValidEmail(),
                loggedUsername: GetValidUsername()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithInvalidDocument()
            => new
            (
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: GetInvalidDocument(),
                email: GetValidEmail(),
                loggedUsername: GetValidUsername()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithInvalidEmail()
            => new
            (
                customerId: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: GetValidDocument(),
                email: "",
                loggedUsername: GetValidUsername()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyLoggedUsername()
           => new
           (
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
