using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Domain.Entities.EmployeePositionHistory
{
    [CollectionDefinition(nameof(EmployeePositionHistoryTestFixtureCollection))]
    public class EmployeePositionHistoryTestFixtureCollection : ICollectionFixture<EmployeePositionHistoryTestFixture>
    { }

    public class EmployeePositionHistoryTestFixture : BaseFixture
    {
        public EmployeePositionHistoryTestFixture()
            : base() { }

        public DomainEntity.EmployeePositionHistory GetValidEmployeePositionHistory()
            => new(Guid.NewGuid(), 10_000, DateTime.UtcNow, null, true);
    }
}
