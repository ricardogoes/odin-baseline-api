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

        public List<Customer> GetValidCustomersList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomer()).ToList();
    }
}
