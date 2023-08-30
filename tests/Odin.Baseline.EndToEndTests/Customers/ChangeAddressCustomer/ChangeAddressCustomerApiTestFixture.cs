using Odin.Baseline.Application.Customers.ChangeAddressCustomer;
using Odin.Baseline.EndToEndTests.Customers.Common;

namespace Odin.Baseline.EndToEndTests.Customers.ChangeAddressCustomer
{
    [CollectionDefinition(nameof(ChangeAddressCustomerApiTestCollection))]
    public class ChangeAddressCustomerApiTestCollection : ICollectionFixture<ChangeAddressCustomerApiTestFixture>
    { }

    public class ChangeAddressCustomerApiTestFixture : CustomerBaseFixture
    {
        public ChangeAddressCustomerApiTestFixture()
            : base()
        { }

        public ChangeAddressCustomerInput GetValidInput(Guid? id = null)
            => new()
            {
                CustomerId = id ?? Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };

        public ChangeAddressCustomerInput GetAddressInputWithoutStreetName(Guid? id = null)
            => new()
            {
                CustomerId = id ?? Guid.NewGuid(),
                StreetName = "",
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };

        public ChangeAddressCustomerInput GetAddressInputWithoutStreetNumber(Guid? id = null)
            => new()
            {
                CustomerId = id ?? Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = -1,
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };

        public ChangeAddressCustomerInput GetAddressInputWithoutNeighborhood(Guid? id = null)
            => new()
            {
                CustomerId = id ?? Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = "",
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };

        public ChangeAddressCustomerInput GetAddressInputWithoutZipCode(Guid? id = null)
            => new()
            {
                CustomerId = id ?? Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = "",
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };

        public ChangeAddressCustomerInput GetAddressInputWithoutCity(Guid? id = null)
            => new()
            {
                CustomerId = id ?? Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = "",
                State = Faker.Address.StateAbbr()
            };

        public ChangeAddressCustomerInput GetAddressInputWithoutState(Guid? id = null)
            => new()
            {
                CustomerId = id ?? Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = ""
            };
    }
}
