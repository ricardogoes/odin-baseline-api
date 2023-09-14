using Bogus;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Positions.Common
{
    public class PositionBaseFixture : BaseFixture
    {
        public string GetValidPositionName()
           => Faker.Commerce.Department();
               
        public Position GetValidPosition(Guid? id = null)
        {
            var department = new Position(id ?? Guid.NewGuid(), GetValidName(), baseSalary: 1_000, isActive: GetRandomBoolean());
            department.Create("unit.testing");

            return department;
        }

        public PositionModel GetValidPositionModel(Guid? customerId = null)
        {
            var position = new PositionModel
            (
                id: Guid.NewGuid(),
                customerId: customerId ?? Guid.NewGuid(),
                name: GetValidPositionName(),
                baseSalary: 10_000,
                isActive: GetRandomBoolean(),
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test"
            );

            return position;
        }

        public List<Position> GetValidPositionsList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidPosition()).ToList();

        public List<PositionModel> GetValidPositionsModelList(Guid? customerId = null, int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidPositionModel(customerId)).ToList();
    }
}
