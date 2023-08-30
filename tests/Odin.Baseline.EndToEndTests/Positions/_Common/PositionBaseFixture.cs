using Bogus;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Positions.Common
{
    public class PositionBaseFixture : BaseFixture
    {
        public string GetValidName()
           => Faker.Commerce.Department();

        public string GetValidUsername()
            => $"{Faker.Name.FirstName().ToLower()}.{Faker.Name.LastName().ToLower()}";

        public Position GetValidPosition(Guid? id = null)
        {
            var department = new Position(id ?? Guid.NewGuid(), GetValidName(), baseSalary: 1_000, isActive: GetRandomBoolean());
            department.Create("unit.testing");

            return department;
        }

        public PositionModel GetValidPositionModel()
        {
            var customer = new PositionModel
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Name = GetValidName(),
                BaseSalary = 10_000,
                IsActive = GetRandomBoolean(),
                CreatedAt = DateTime.Now,
                CreatedBy = "unit.test",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "unit.test"
            };

            return customer;
        }

        public List<Position> GetValidPositionsList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidPosition()).ToList();

        public List<PositionModel> GetValidPositionsModelList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidPositionModel()).ToList();
    }
}
