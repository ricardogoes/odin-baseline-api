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
            => new()
            {
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidFirstName(),
                LastName = GetValidLastName(),
                Document = GetValidDocument(),
                Email = GetValidEmail(),
                LoggedUsername = GetValidUsername()
            };

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyFirstName()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = "",
                LastName = GetValidLastName(),
                Document = GetValidDocument(),
                Email = GetValidEmail(),
                LoggedUsername = GetValidUsername()
            };

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyLastName()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidFirstName(),
                LastName = "",
                Document = GetValidDocument(),
                Email = GetValidEmail(),
                LoggedUsername = GetValidUsername()
            };

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyDocument()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidFirstName(),
                LastName = GetValidLastName(),
                Document = "",
                Email = GetValidEmail(),
                LoggedUsername = GetValidUsername()
            };

        public CreateEmployeeInput GetCreateEmployeeInputWithInvalidDocument()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidFirstName(),
                LastName = GetValidLastName(),
                Document = GetInvalidDocument(),
                Email = GetValidEmail(),
                LoggedUsername = GetValidUsername()
            };

        public CreateEmployeeInput GetCreateEmployeeInputWithInvalidEmail()
            => new()
            {
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidFirstName(),
                LastName = GetValidLastName(),
                Document = GetValidDocument(),
                Email = "",
                LoggedUsername = GetValidUsername()
            };

        public CreateEmployeeInput GetCreateEmployeeInputWithEmptyLoggedUsername()
           => new()
           {
               CustomerId = Guid.NewGuid(),
               DepartmentId = Guid.NewGuid(),
               FirstName = GetValidFirstName(),
               LastName = GetInvalidDocument(),
               Document = GetValidDocument(),
               Email = GetValidEmail(),
               LoggedUsername = ""
           };
    }


}
