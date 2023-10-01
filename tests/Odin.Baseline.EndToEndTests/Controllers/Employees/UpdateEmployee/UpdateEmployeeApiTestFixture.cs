using Odin.Baseline.Application.Employees.UpdateEmployee;
using Odin.Baseline.EndToEndTests.Controllers.Employees;

namespace Odin.Baseline.EndToEndTests.Controllers.Employees.UpdateEmployee
{
    [CollectionDefinition(nameof(UpdateEmployeeApiTestCollection))]
    public class UpdateEmployeeApiTestCollection : ICollectionFixture<UpdateEmployeeApiTestFixture>
    { }

    public class UpdateEmployeeApiTestFixture : EmployeeBaseFixture
    {
        public UpdateEmployeeApiTestFixture()
            : base()
        { }

        public UpdateEmployeeInput GetValidUpdateEmployeeInput(Guid? id = null, Guid? departmentId = null)
           => new
           (
               id: id ?? Guid.NewGuid(),
               departmentId: departmentId,
               firstName: GetValidEmployeeFirstName(),
               lastName: GetValidEmployeeLastName(),
               document: GetValidEmployeeDocument(),
               email: GetValidEmployeeEmail()
           );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyFirstName()
            => new
            (
                id: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: "",
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyLastName()
            => new
            (
                id: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: "",
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyDocument()
            => new
            (
                id: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: "",
                email: GetValidEmployeeEmail()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithInvalidDocument()
            => new
            (
                id: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: "12.123.123/0002-10",
                email: GetValidEmployeeEmail()
            );

        public UpdateEmployeeInput GetUpdateEmployeeInputWithInvalidEmail()
            => new
            (
                id: Guid.NewGuid(),
                departmentId: Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: ""
            );
    }
}
