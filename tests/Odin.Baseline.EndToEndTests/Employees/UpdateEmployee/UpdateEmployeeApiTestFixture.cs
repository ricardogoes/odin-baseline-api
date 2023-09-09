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
           => new()
           {
               Id = id ?? Guid.NewGuid(),
               CustomerId = customerId ?? Guid.NewGuid(),
               DepartmentId = departmentId,
               FirstName = GetValidEmployeeFirstName(),
               LastName = GetValidEmployeeLastName(),
               Document = GetValidEmployeeDocument(),
               Email = GetValidEmployeeEmail(),
               LoggedUsername = "unit.testing"
           };

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyFirstName()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = "",
                LastName = GetValidEmployeeLastName(),
                Document = GetValidEmployeeDocument(),
                Email = GetValidEmployeeEmail(),
                LoggedUsername = "unit.testing"
            };

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyLastName()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidEmployeeFirstName(),
                LastName = "",
                Document = GetValidEmployeeDocument(),
                Email = GetValidEmployeeEmail(),
                LoggedUsername = "unit.testing"
            };

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyDocument()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidEmployeeFirstName(),
                LastName = GetValidEmployeeLastName(),
                Document = "",
                Email = GetValidEmployeeEmail(),
                LoggedUsername = "unit.testing"
            };

        public UpdateEmployeeInput GetUpdateEmployeeInputWithInvalidDocument()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidEmployeeFirstName(),
                LastName = GetValidEmployeeLastName(),
                Document = "12.123.123/0002-10",
                Email = GetValidEmployeeEmail(),
                LoggedUsername = "unit.testing"
            };

        public UpdateEmployeeInput GetUpdateEmployeeInputWithInvalidEmail()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidEmployeeFirstName(),
                LastName = GetValidEmployeeLastName(),
                Document = GetValidEmployeeDocument(),
                Email = "",
                LoggedUsername = "unit.testing"
            };

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyLoggedUsername()
           => new()
           {
               CustomerId = Guid.NewGuid(),
               DepartmentId = Guid.NewGuid(),
               FirstName = GetValidEmployeeFirstName(),
               LastName = "12.123.123/0002-10",
               Document = GetValidEmployeeDocument(),
               Email = GetValidEmployeeEmail(),
               LoggedUsername = ""
           };
    }
}
