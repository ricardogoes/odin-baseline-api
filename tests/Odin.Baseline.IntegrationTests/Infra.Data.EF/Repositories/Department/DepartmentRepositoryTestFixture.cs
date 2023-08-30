using Odin.Baseline.Infra.Data.EF.Models;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.IntegrationTests.Infra.Data.EF.Repositories.Department
{
    [CollectionDefinition(nameof(DepartmentRepositoryTestFixtureCollection))]
    public class DepartmentRepositoryTestFixtureCollection : ICollectionFixture<DepartmentRepositoryTestFixture>
    { }

    public class DepartmentRepositoryTestFixture : BaseFixture
    {
        public DepartmentRepositoryTestFixture()
            : base()
        { }

        public string GetValidName()
            => Faker.Commerce.Department();

        public DomainEntity.Department GetValidDepartment()
        {
            var department = new DomainEntity.Department(Guid.NewGuid(), GetValidName(), isActive: true);
            department.Create("unit.testing");

            return department;
        }

        public DepartmentModel GetValidDepartmentModel(Guid? customerId = null)
        {
            var department = new DepartmentModel
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId ?? Guid.NewGuid(),
                Name = GetValidName(),
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "unit.test",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "unit.test"
            };

            return department;
        }

        public List<DomainEntity.Department> GetValidDepartmentsList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidDepartment()).ToList();

        public List<DepartmentModel> GetValidDepartmentsModelList(List<Guid> customersIds, int length = 10)
        {
            var departments = new List<DepartmentModel>();
            customersIds.ForEach(customerId =>
            {
                departments.AddRange(Enumerable.Range(1, length).Select(_ => GetValidDepartmentModel(customerId)).ToList());
            });

            return departments;
        }
    }
}
