using Bogus.Extensions.Brazil;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Domain.Entities.Department
{
    public class DepartmentTestFixture : BaseFixture
    {
        public DepartmentTestFixture()
            : base() { }

        public string GetValidDepartmentName()
            => Faker.Company.CompanyName(1);

        public string GetValidDepartmentDocument()
            => Faker.Company.Cnpj();

        public DomainEntity.Department GetValidDepartment()
        {
            var department = new DomainEntity.Department(Guid.NewGuid(), GetValidDepartmentName());
            department.Create();

            return department;
        }
    }

    [CollectionDefinition(nameof(DepartmentTestFixture))]
    public class DepartmentTestFixtureCollection
        : ICollectionFixture<DepartmentTestFixture>
    { }
}
