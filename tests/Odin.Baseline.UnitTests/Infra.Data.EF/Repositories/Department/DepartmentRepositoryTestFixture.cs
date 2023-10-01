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

        public DepartmentModel GetValidDepartmentModel()
        {            
            var model = new DepartmentModel
            (
                id: Guid.NewGuid(),
                name: GetValidDepartmentName(),
                isActive: true,
                createdAt: DateTime.UtcNow,
                createdBy: "unit.testing",
                lastUpdatedAt: DateTime.UtcNow,
                lastUpdatedBy: "unit.testing",
                tenantId: TenantId
            );

            model.SetAuditLog("unit.testing", created: true);

            return model;
        }

        public List<DomainEntity.Department> GetValidDepartmentsList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidDepartment()).ToList();

        public List<DepartmentModel> GetValidDepartmentsModelList(int length = 10)
        {
            var departments = new List<DepartmentModel>();
            departments.AddRange(Enumerable.Range(1, length).Select(_ => GetValidDepartmentModel()).ToList());
            return departments;
        }
    }
}
