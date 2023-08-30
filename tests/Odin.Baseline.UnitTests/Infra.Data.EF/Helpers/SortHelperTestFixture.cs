namespace Odin.Baseline.UnitTests.Infra.Data.EF.Helpers
{
    [CollectionDefinition(nameof(SortHelperTestFixtureCollection))]
    public class SortHelperTestFixtureCollection : ICollectionFixture<SortHelperTestFixture>
    { }

    public class SortHelperTestFixture : BaseFixture
    {
    }
}
