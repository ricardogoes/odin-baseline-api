using Odin.Baseline.Application.Customers.ChangeAddressCustomer;
using Odin.Baseline.UnitTests.Application.Customers.Common;

namespace Odin.Baseline.UnitTests.Application.Customers.ChangeAddressCustomer
{
    [CollectionDefinition(nameof(ChangeAddressCustomerTestFixtureCollection))]
    public class ChangeAddressCustomerTestFixtureCollection : ICollectionFixture<ChangeAddressCustomerTestFixture>
    { }

    public class ChangeAddressCustomerTestFixture : CustomerBaseFixture
    {
        public ChangeAddressCustomerTestFixture()
            : base() { }

        public ChangeAddressCustomerInput GetValidInputAddress()
        {
            var address = new ChangeAddressCustomerInput
            {
                CustomerId = Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressCustomerInput GetInputAddressWithCustomerIdEmpty()
        {
            var address = new ChangeAddressCustomerInput
            {
                CustomerId = Guid.Empty,
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressCustomerInput GetInputAddressWithStreetNameEmpty()
        {
            var address = new ChangeAddressCustomerInput
            {
                CustomerId = Guid.NewGuid(),
                StreetName = string.Empty,
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressCustomerInput GetInputAddressWithStreetNumberEmpty()
        {
            var address = new ChangeAddressCustomerInput
            {
                CustomerId = Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = 0,
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressCustomerInput GetInputAddressWithNeighborhoodEmpty()
        {
            var address = new ChangeAddressCustomerInput
            {
                CustomerId = Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = string.Empty,
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressCustomerInput GetInputAddressWithZipCodeEmpty()
        {
            var address = new ChangeAddressCustomerInput
            {
                CustomerId = Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = 196,
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = string.Empty,
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressCustomerInput GetInputAddressWithCityEmpty()
        {
            var address = new ChangeAddressCustomerInput
            {
                CustomerId = Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = string.Empty,
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressCustomerInput GetInputAddressWithStateEmpty()
        {
            var address = new ChangeAddressCustomerInput
            {
                CustomerId = Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = string.Empty
            };
            return address;
        }
    }
}
