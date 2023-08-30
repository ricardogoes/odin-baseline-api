using Bogus;
using Bogus.Extensions.Brazil;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Customers.Common
{
    public class CustomerBaseFixture : BaseFixture
    {
        public string GetValidName()
           => Faker.Company.CompanyName(1);

        public string GetValidDocument()
            => Faker.Company.Cnpj();

        public string GetInvalidDocument()
           => "12.123.123/0002-12";

        public string GetValidUsername()
            => $"{Faker.Name.FirstName().ToLower()}.{Faker.Name.LastName().ToLower()}";

        public Customer GetValidCustomer()
        {
            var customer = new Customer(GetValidName(), GetValidDocument(), isActive: GetRandomBoolean());
            customer.Create("unit.testing");

            return customer;
        }

        public CustomerModel GetValidCustomerModel()
        {
            var customer = new CustomerModel
            {
                Id = Guid.NewGuid(),
                Name = GetValidName(),
                Document = GetValidDocument(),
                IsActive = GetRandomBoolean(),
                CreatedAt = DateTime.Now,
                CreatedBy = "unit.test",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "unit.test"
            };

            return customer;
        }

        public List<Customer> GetValidCustomersList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomer()).ToList();

        public List<CustomerModel> GetValidCustomersModelList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomerModel()).ToList();
    }
}
