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
            => new()
            {
                CustomerId = customerId ?? Guid.NewGuid(),
                DepartmentId = departmentId,
                FirstName = GetValidEmployeeFirstName(),
                LastName = GetValidEmployeeLastName(),
                Document = GetValidEmployeeDocument(),
                Email = GetValidEmployeeEmail(),
                LoggedUsername = "unit.testing"
            };

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyFirstName()
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

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyLastName()
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

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyDocument()
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

        public CreateEmployeeInput GetCreateEmployeeInputWithInvalidDocument()
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

        public CreateEmployeeInput GetCreateEmployeeInputWithInvalidEmail()
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

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyLoggedUsername()
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
