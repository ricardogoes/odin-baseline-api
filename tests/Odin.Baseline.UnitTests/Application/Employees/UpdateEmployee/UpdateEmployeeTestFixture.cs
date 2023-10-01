using Odin.Baseline.Application.Employees.UpdateEmployee;

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
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: GetValidDocument(),
                email: GetValidEmail()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyFirstName(Guid? id = null)
            => new
            (
                id:  id ?? Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: "",
                lastName: GetValidLastName(),
                document: GetValidDocument(),
                email: GetValidEmail()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyLastName(Guid? id = null)
            => new
            (
                id:  id ?? Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: "",
                document: GetValidDocument(),
                email: GetValidEmail()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyDocument(Guid? id = null)
            => new
            (
                id: id ?? Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: "",
                email: GetValidEmail()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithInvalidDocument(Guid? id = null)
            => new
            (
                id:  id ?? Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: GetInvalidDocument(),
                email: GetValidEmail()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithInvalidEmail(Guid? id = null)
            => new
            (
                id:  id ?? Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidFirstName(),
                lastName: GetValidLastName(),
                document: GetValidDocument(),
                email: ""
            );
    }
}
