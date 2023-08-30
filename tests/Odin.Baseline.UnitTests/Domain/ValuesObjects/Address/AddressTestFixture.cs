using ValueObject = Odin.Baseline.Domain.ValueObjects;

namespace Odin.Baseline.UnitTests.Domain.ValuesObjects.Address
{
    [CollectionDefinition(nameof(AddressTestFixtureCollection))]
    public class AddressTestFixtureCollection : ICollectionFixture<AddressTestFixture>
    { }

    public class AddressTestFixture : BaseFixture
    {
        public AddressTestFixture()
            : base() { }

        public string GetValidStreetName()
            => Faker.Address.StreetName();

        public int GetValidStreetNumber()
             => int.Parse(Faker.Address.BuildingNumber());

        public string GetValidComplement()
            => Faker.Address.SecondaryAddress();

        public string GetValidNeighborhood()
            => Faker.Address.CardinalDirection();

        public string GetValidZipCode()
            => Faker.Address.ZipCode();

        public string GetValidCity()
            => Faker.Address.City();

        public string GetValidState()
            => Faker.Address.StateAbbr();

        public ValueObject.Address GetValidAddress()
        {
            var address = new ValueObject.Address(
                GetValidStreetName(),
                GetValidStreetNumber(),
                GetValidComplement(),
                GetValidNeighborhood(),
                GetValidZipCode(),
                GetValidCity(),
                GetValidState()
            );
            return address;
        }
    }
}
