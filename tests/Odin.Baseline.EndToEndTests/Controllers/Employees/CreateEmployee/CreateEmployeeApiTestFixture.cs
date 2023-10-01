using Odin.Baseline.Application.Employees.CreateEmployee;
using Odin.Baseline.EndToEndTests.Controllers.Employees;

namespace Odin.Baseline.EndToEndTests.Controllers.Employees.CreateEmployee
{
    [CollectionDefinition(nameof(CreateEmployeeApiTestCollection))]
    public class CreateEmployeeApiTestCollection : ICollectionFixture<CreateEmployeeApiTestFixture>
    { }

    public class CreateEmployeeApiTestFixture : EmployeeBaseFixture
    {
        public CreateEmployeeApiTestFixture()
            : base()
        { }

        public CreateEmployeeInput GetValidCreateEmployeeInput(Guid? departmentId = null)
            => new
            (
                departmentId: departmentId,
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyFirstName()
            => new
            (
                departmentId: Guid.NewGuid(),
                firstName: "",
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyLastName()
            => new
            (
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: "",
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyDocument()
            => new
            (
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: "",
                email: GetValidEmployeeEmail()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithInvalidDocument()
            => new
            (
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: "12.123.123/0002-10",
                email: GetValidEmployeeEmail()
            );

        public CreateEmployeeInput GetCreateEmployeeInputWithInvalidEmail()
            => new
            (
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: ""
            );
    }
}
