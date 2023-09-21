using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Domain.Entities.Position
{
    [CollectionDefinition(nameof(PositionTestFixtureCollection))]
    public class PositionTestFixtureCollection : ICollectionFixture<PositionTestFixture>
    { }

    public class PositionTestFixture : BaseFixture
    {
        public PositionTestFixture()
            : base() { }

        public string GetValidPositionName()
            => Faker.Company.CompanyName(1);

        public decimal GetValidBaseSalary()
            => 10_000;

        public  DomainEntity.Position GetValidPosition(Guid? customerId = null)
        {
            var position = new DomainEntity.Position(customerId ?? Guid.NewGuid(), GetValidPositionName(), GetValidBaseSalary());
            position.Create("unit.testing");

            return position;
        }
    }
}
