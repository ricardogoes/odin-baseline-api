using Odin.Baseline.Application.Departments.GetDepartments;
using Odin.Baseline.UnitTests.Application.Departments.Common;

namespace Odin.Baseline.UnitTests.Application.Departments.GetDepartments
{
    [CollectionDefinition(nameof(GetDepartmentsTestFixtureCollection))]
    public class GetDepartmentsTestFixtureCollection : ICollectionFixture<GetDepartmentsTestFixture>
    { }

    public class GetDepartmentsTestFixture : DepartmentBaseFixture
    {
        public GetDepartmentsInput GetValidGetDepartmentsInput(Guid? id = null)
            => new()
            {
                CustomerId = id ?? Guid.NewGuid(),
                Name = Faker.Commerce.Department(),
                IsActive = true
            };
    }
}
