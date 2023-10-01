using Moq;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.UnitTests.Application.Departments
{
    public abstract class DepartmentBaseFixture : BaseFixture
    {

        public Mock<IRepository<Department>> GetRepositoryMock()
            => new();

        public Mock<IUnitOfWork> GetUnitOfWorkMock()
            => new();

        public string GetValidName()
            => Faker.Company.CompanyName(1);

        public List<Department> GetValidDepartmentsList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidDepartment()).ToList();
    }
}
