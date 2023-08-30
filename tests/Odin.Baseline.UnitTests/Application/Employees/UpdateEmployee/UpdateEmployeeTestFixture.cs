using Odin.Baseline.Application.Employees.UpdateEmployee;
using Odin.Baseline.UnitTests.Application.Employees.Common;

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
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidFirstName(),
                LastName = GetValidLastName(),
                Document = GetValidDocument(),
                Email = GetValidEmail(),
                LoggedUsername = GetValidUsername()
            };

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyFirstName(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = "",
                LastName = GetValidLastName(),
                Document = GetValidDocument(),
                Email = GetValidEmail(),
                LoggedUsername = GetValidUsername()
            };

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyLastName(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidFirstName(),
                LastName = "",
                Document = GetValidDocument(),
                Email = GetValidEmail(),
                LoggedUsername = GetValidUsername()
            };

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyDocument(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidFirstName(),
                LastName = GetValidLastName(),
                Document = "",
                Email = GetValidEmail(),
                LoggedUsername = GetValidUsername()
            };

        public UpdateEmployeeInput GetUpdateEmployeeInputWithInvalidDocument(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidFirstName(),
                LastName = GetValidLastName(),
                Document = GetInvalidDocument(),
                Email = GetValidEmail(),
                LoggedUsername = GetValidUsername()
            };

        public UpdateEmployeeInput GetUpdateEmployeeInputWithInvalidEmail(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                FirstName = GetValidFirstName(),
                LastName = GetValidLastName(),
                Document = GetValidDocument(),
                Email = "",
                LoggedUsername = GetValidUsername()
            };

        public UpdateEmployeeInput GetUpdateEmployeeInputWithEmptyLoggedUsername(Guid? id = null)
           => new()
           {
               Id = id ?? Guid.NewGuid(),
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
