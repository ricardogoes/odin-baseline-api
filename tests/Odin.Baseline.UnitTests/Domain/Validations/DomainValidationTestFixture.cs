using Bogus;

namespace Odin.Baseline.UnitTests.Domain.Validations
{
    public class DomainValidationTestFixture : BaseFixture
    {
        public DomainValidationTestFixture()
            : base() { }

        public string GetValidFieldName()
            => Faker.Commerce.ProductName().Replace(" ", "");

        public string GetValidValue()
           => Faker.Commerce.ProductName();

    }

    [CollectionDefinition(nameof(DomainValidationTestFixture))]
    public class DomainValidationTestFixtureCollection
        : ICollectionFixture<DomainValidationTestFixture>
    { }
}
