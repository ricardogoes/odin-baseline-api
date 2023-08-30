using Bogus.Extensions.Brazil;
using Odin.Baseline.Infra.Data.EF.Models;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.IntegrationTests.Infra.Data.EF.Repositories.Customer
{
    [CollectionDefinition(nameof(CustomerRepositoryTestFixtureCollection))]
    public class CustomerRepositoryTestFixtureCollection : ICollectionFixture<CustomerRepositoryTestFixture>
    { }

    public class CustomerRepositoryTestFixture : BaseFixture
    {
        public CustomerRepositoryTestFixture()
            : base()
        { }

        public string GetValidName()
            => Faker.Company.CompanyName(1);

        public string GetValidDocument()
            => Faker.Company.Cnpj();

        public DomainEntity.Customer GetValidCustomer()
        {
            var customer = new DomainEntity.Customer(GetValidName(), GetValidDocument(), isActive: true);
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
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "unit.test",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "unit.test"
            };

            return customer;
        }

        public List<DomainEntity.Customer> GetValidCustomersList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomer()).ToList();

        public List<CustomerModel> GetValidCustomersModelList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomerModel()).ToList();
    }
}
