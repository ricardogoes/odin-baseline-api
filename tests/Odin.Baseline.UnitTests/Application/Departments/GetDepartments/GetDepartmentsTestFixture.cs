using Odin.Baseline.Application.Departments.GetDepartments;

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
                Name = Faker.Commerce.Department(),
                IsActive = true
            };
    }
}
