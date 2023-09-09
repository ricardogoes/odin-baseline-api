using Bogus;
using Bogus.Extensions.Brazil;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.SeedWork;
using Odin.Baseline.Domain.ValueObjects;

namespace Odin.Baseline.UnitTests
{
    public abstract class BaseFixture
    {
        protected Faker Faker { get; set; }

        protected BaseFixture()
            => Faker = new Faker("pt_BR");

        public string GetValidUsername()
            => $"{Faker.Name.FirstName().ToLower()}.{Faker.Name.LastName().ToLower()}";

        public static string GetInvalidUsersername()
            => "";

        public static bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public string GetValidCustomerName()
            => Faker.Company.CompanyName(1);

        public string GetValidCustomerDocument()
            => Faker.Company.Cnpj();

        public Customer GetValidCustomer()
        {
            var customer = new Customer(GetValidCustomerName(), GetValidCustomerDocument());
            customer.Create();
            return customer;
        }

        public Address GetValidAddress()
        {
            var address = new Address(
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

        public string GetValidDepartmentName()
            => Faker.Company.CompanyName(1);

        public string GetValidDepartmentDocument()
            => Faker.Company.Cnpj();

        public Department GetValidDepartment(Guid? customerId = null)
        {
            var department = new Department(customerId ?? Guid.NewGuid(), GetValidDepartmentName());
            department.Create();

            return department;
        }
    }
}

