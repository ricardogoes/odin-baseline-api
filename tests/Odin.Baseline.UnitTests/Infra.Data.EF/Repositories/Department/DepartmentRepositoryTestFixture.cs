using Odin.Baseline.Infra.Data.EF.Models;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Infra.Data.EF.Repositories.Department
{
    [CollectionDefinition(nameof(DepartmentRepositoryTestFixtureCollection))]
    public class DepartmentRepositoryTestFixtureCollection : ICollectionFixture<DepartmentRepositoryTestFixture>
    { }

    public class DepartmentRepositoryTestFixture : BaseFixture
    {
        public DepartmentRepositoryTestFixture()
            : base()
        { }

        public DepartmentModel GetValidDepartmentModel(Guid? customerId = null)
        {
            return new DepartmentModel
            (
                id: Guid.NewGuid(),
                customerId: customerId ?? Guid.NewGuid(),
                name: GetValidDepartmentName(),
                isActive: true,
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test"
            );
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
