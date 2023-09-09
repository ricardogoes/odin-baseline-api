using Odin.Baseline.Infra.Data.EF.Models;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.IntegrationTests.Infra.Data.EF.Repositories.Position
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

        public DomainEntity.Position GetValidPosition(Guid? customerId = null)
        {
            var position = new DomainEntity.Position(customerId ?? Guid.NewGuid(), GetValidName(), 10_000, isActive: true);
            position.Create("unit.testing");

            return position;
        }

        public PositionModel GetValidPositionModel(Guid? customerId = null)
        {
            var position = new PositionModel
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId ?? Guid.NewGuid(),
                Name = GetValidName(),
                BaseSalary = 10_000,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "unit.test",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "unit.test"
            };

            return position;
        }

        public List<DomainEntity.Position> GetValidPositionsList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidPosition()).ToList();

        public List<PositionModel> GetValidPositionsModelList(List<Guid> customersIds, int length = 10)
        {
            var positions = new List<PositionModel>();
            customersIds.ForEach(customerId =>
            {
                positions.AddRange(Enumerable.Range(1, length).Select(_ => GetValidPositionModel(customerId)).ToList());
            });

            return positions;
        }
    }
}
