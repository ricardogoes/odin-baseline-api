using Odin.Baseline.Infra.Data.EF.Models;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Infra.Data.EF.Repositories.Customer
{
    [CollectionDefinition(nameof(CustomerRepositoryTestFixtureCollection))]
    public class CustomerRepositoryTestFixtureCollection : ICollectionFixture<CustomerRepositoryTestFixture>
    { }

    public class CustomerRepositoryTestFixture : BaseFixture
    {
        public CustomerRepositoryTestFixture()
            : base()
        { }

        public List<DomainEntity.Customer> GetValidCustomersList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomer()).ToList();

        public List<CustomerModel> GetValidCustomersModelList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomerModel()).ToList();
    }
}
