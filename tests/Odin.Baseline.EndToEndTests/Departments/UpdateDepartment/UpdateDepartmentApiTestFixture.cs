using Odin.Baseline.Application.Departments.UpdateDepartment;
using Odin.Baseline.EndToEndTests.Departments.Common;

namespace Odin.Baseline.EndToEndTests.Departments.UpdateDepartment
{
    [CollectionDefinition(nameof(UpdateDepartmentApiTestCollection))]
    public class UpdateDepartmentApiTestCollection : ICollectionFixture<UpdateDepartmentApiTestFixture>
    { }

    public class UpdateDepartmentApiTestFixture : DepartmentBaseFixture
    {
        public UpdateDepartmentApiTestFixture()
            : base()
        { }

        public UpdateDepartmentInput GetValidInput(Guid id)
            => new()
            {
                Id = id,
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                LoggedUsername = GetValidUsername()
            };

        public UpdateDepartmentInput GetInputWithIdEmpty()
            => new()
            {
                Id = Guid.Empty,
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                LoggedUsername = GetValidUsername()
            };


        public UpdateDepartmentInput GetInputWithNameEmpty(Guid id)
            => new()
            {
                Id = id,
                CustomerId = Guid.NewGuid(),
                Name = "",
                LoggedUsername = GetValidUsername()
            };

        public UpdateDepartmentInput GetInputWithCustomerIdEmpty(Guid id)
            => new()
            {
                Id = id,
                CustomerId = Guid.Empty,
                Name = "",
                LoggedUsername = GetValidUsername()
            };

        public UpdateDepartmentInput GetInputWithUsernameEmpty(Guid id)
            => new()
            {
                Id = id,
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                LoggedUsername = ""
            };
    }
}
