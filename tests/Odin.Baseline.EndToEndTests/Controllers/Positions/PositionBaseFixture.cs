using Bogus;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Controllers.Positions
{
    public class PositionBaseFixture : BaseFixture
    {
        public string GetValidPositionName()
           => Faker.Commerce.Department();

        public Position GetValidPosition()
        {
            var department = new Position(GetValidPositionName(), baseSalary: 1_000, isActive: GetRandomBoolean());

            return department;
        }

        public PositionModel GetValidPositionModel(Guid? customerId = null)
        {
            var position = new PositionModel
            (
                id: Guid.NewGuid(),
                name: GetValidPositionName(),
                baseSalary: 10_000,
                isActive: GetRandomBoolean(),
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test",
                tenantId: TenantSinapseId
            );

            return position;
        }

        public List<Position> GetValidPositionsList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidPosition()).ToList();

        public List<PositionModel> GetValidPositionsModelList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidPositionModel()).ToList();
    }
}
