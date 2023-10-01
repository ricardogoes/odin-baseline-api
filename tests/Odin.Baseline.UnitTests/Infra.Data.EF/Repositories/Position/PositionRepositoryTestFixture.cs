using Odin.Baseline.Infra.Data.EF.Models;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Infra.Data.EF.Repositories.Position
{
    [CollectionDefinition(nameof(PositionRepositoryTestFixtureCollection))]
    public class PositionRepositoryTestFixtureCollection : ICollectionFixture<PositionRepositoryTestFixture>
    { }

    public class PositionRepositoryTestFixture : BaseFixture
    {
        public PositionRepositoryTestFixture()
            : base()
        { }

        public string GetValidName()
            => Faker.Commerce.Department();

        public DomainEntity.Position GetValidPosition()
        {
            var position = new DomainEntity.Position(GetValidName(), 10_000, isActive: true);
            return position;
        }

        public PositionModel GetValidPositionModel()
        {
            return new PositionModel
            (
                id: Guid.NewGuid(),                
                name: GetValidName(),
                baseSalary: 10_000,
                isActive: true,
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test",
                tenantId: TenantId
            );
        }

        public List<DomainEntity.Position> GetValidPositionsList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidPosition()).ToList();

        public List<PositionModel> GetValidPositionsModelList(int length = 10)
        {
            var positions = new List<PositionModel>();
            positions.AddRange(Enumerable.Range(1, length).Select(_ => GetValidPositionModel()).ToList());
            return positions;
        }
    }
}
