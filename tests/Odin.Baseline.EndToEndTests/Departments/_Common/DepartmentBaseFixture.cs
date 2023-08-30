using Bogus;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Departments.Common
{
    public class DepartmentBaseFixture : BaseFixture
    {
        public string GetValidName()
           => Faker.Commerce.Department();

        public string GetValidUsername()
            => $"{Faker.Name.FirstName().ToLower()}.{Faker.Name.LastName().ToLower()}";

        public Department GetValidDepartment(Guid? id = null)
        {
            var department = new Department(id ?? Guid.NewGuid(), GetValidName(), isActive: GetRandomBoolean());
            department.Create("unit.testing");

            return department;
        }

        public DepartmentModel GetValidDepartmentModel()
        {
            var customer = new DepartmentModel
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                IsActive = GetRandomBoolean(),
                CreatedAt = DateTime.Now,
                CreatedBy = "unit.test",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "unit.test"
            };

            return customer;
        }

        public List<Department> GetValidDepartmentsList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidDepartment()).ToList();

        public List<DepartmentModel> GetValidDepartmentsModelList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidDepartmentModel()).ToList();
    }
}
