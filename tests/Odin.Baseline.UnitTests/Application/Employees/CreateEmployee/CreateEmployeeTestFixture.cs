using Odin.Baseline.Application.Employees.CreateEmployee;

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
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: GetValidDocument(),
                email: GetValidEmail()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyFirstName()
            => new
            (
                departmentId: Guid.NewGuid(),
                firstName: "",
                lastName: GetValidLastName(),
                document: GetValidDocument(),
                email: GetValidEmail()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyLastName()
            => new
            (
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: "",
                document: GetValidDocument(),
                email: GetValidEmail()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyDocument()
            => new
            (
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: "",
                email: GetValidEmail()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithInvalidDocument()
            => new
            (
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: GetInvalidDocument(),
                email: GetValidEmail()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithInvalidEmail()
            => new
            (
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: GetValidDocument(),
                email: ""
            );
    }


}
