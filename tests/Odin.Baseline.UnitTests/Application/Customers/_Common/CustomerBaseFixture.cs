using Bogus.Extensions.Brazil;
using Moq;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.UnitTests.Application.Customers.Common
{
    public abstract class CustomerBaseFixture : BaseFixture
    {
        public Mock<IDocumentService> GetDocumentServiceMock()
            => new();
        public Mock<ICustomerRepository> GetRepositoryMock()
            => new();

        public Mock<IUnitOfWork> GetUnitOfWorkMock()
            => new();

        public string GetValidName()
            => Faker.Company.CompanyName(1);

        public string GetValidDocument()
            => Faker.Company.Cnpj();

        public static string GetInvalidDocument()
            => "12.123.123/0002-12";

        public Customer GetValidCustomer()
        {
            var customer = new Customer(GetValidName(), GetValidDocument(), isActive: GetRandomBoolean());
            customer.Create();

            return customer;
        }

        public List<Customer> GetValidCustomersList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomer()).ToList();
    }
}
