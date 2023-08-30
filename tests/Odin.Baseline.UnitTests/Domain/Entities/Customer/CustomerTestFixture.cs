using Bogus.Extensions.Brazil;
using DomainEntity = Odin.Baseline.Domain.Entities;
using ValueObject = Odin.Baseline.Domain.ValueObjects;

namespace Odin.Baseline.UnitTests.Domain.Entities.Customer
{
    public class CustomerTestFixture : BaseFixture
    {
        public CustomerTestFixture()
            : base() { }

        public string GetValidCustomerName()
            => Faker.Company.CompanyName(1);

        public string GetValidCustomerDocument()
            => Faker.Company.Cnpj();

        public DomainEntity.Customer GetValidCustomer()
        {
            var customer = new DomainEntity.Customer(GetValidCustomerName(), GetValidCustomerDocument());
            customer.Create();
            return customer;
        }

        public ValueObject.Address GetValidAddress()
        {
            var address = new ValueObject.Address(
                Faker.Address.StreetName(),
                int.Parse(Faker.Address.BuildingNumber()),
                Faker.Address.SecondaryAddress(),
                Faker.Address.CardinalDirection(),
                Faker.Address.ZipCode(),
                Faker.Address.City(),
                Faker.Address.StateAbbr()
            );
            return address;
        }
    }

    [CollectionDefinition(nameof(CustomerTestFixture))]
    public class CustomerTestFixtureCollection
        : ICollectionFixture<CustomerTestFixture>
    { }
}
