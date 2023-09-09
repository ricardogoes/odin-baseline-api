namespace Odin.Baseline.UnitTests.Domain.Entities.Department
{
    public class DepartmentTestFixture : BaseFixture
    {
        public DepartmentTestFixture()
            : base() { }
    }

    [CollectionDefinition(nameof(DepartmentTestFixture))]
    public class DepartmentTestFixtureCollection
        : ICollectionFixture<DepartmentTestFixture>
    { }
}
