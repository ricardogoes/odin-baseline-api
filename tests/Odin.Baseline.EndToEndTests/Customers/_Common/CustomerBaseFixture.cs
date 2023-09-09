using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Customers.Common
{
    public class CustomerBaseFixture : BaseFixture
    {
        public List<Customer> GetValidCustomersList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomer()).ToList();

        public List<CustomerModel> GetValidCustomersModelList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomerModel()).ToList();
    }
}
