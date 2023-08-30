using FluentAssertions;
using Moq;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Infra.Data.EF.Mappers;
using Service = Odin.Baseline.Domain.Services;

namespace Odin.Baseline.UnitTests.Domain.Services.DocumentService
{
    [Collection(nameof(DocumentServiceTestFixtureCollection))]
    public class DocumentServiceTest
    {
        private readonly DocumentServiceTestFixture _fixture;

        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;

        public DocumentServiceTest(DocumentServiceTestFixture fixture)
        {
            _fixture = fixture;

            _customerRepositoryMock = _fixture.GetCustomerRepository();
            _employeeRepositoryMock = _fixture.GetEmployeeRepository();
        }

        [Fact(DisplayName = "IsDocumentUnique() should return true when customer document does not exist")]
        [Trait("Domain", "Services / DocumentService")]
        public async Task IsCustomerDocumentUnique()
        {
            var customer = _fixture.GetValidCustomer();
            
            _customerRepositoryMock.Setup(s => s.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new NotFoundException());

            var documentService = new Service.DocumentService(_customerRepositoryMock.Object, _employeeRepositoryMock.Object);
            var output = await documentService.IsDocumentUnique(customer, CancellationToken.None);

            output.Should().BeTrue();

            _customerRepositoryMock.Verify(x => x.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "IsDocumentUnique() should return true when customer exist but with same id")]
        [Trait("Domain", "Services / DocumentService")]
        public async Task IsCustomerDocumentUniqueWithSameIds()
        {
            var customer = _fixture.GetValidCustomer();
            
            _customerRepositoryMock.Setup(s => s.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(customer));

            var documentService = new Service.DocumentService(_customerRepositoryMock.Object, _employeeRepositoryMock.Object);
            var output = await documentService.IsDocumentUnique(customer, CancellationToken.None);

            output.Should().BeTrue();

            _customerRepositoryMock.Verify(x => x.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "IsDocumentUnique() should return false when customer already exists")]
        [Trait("Domain", "Services / DocumentService")]
        public async Task IsCustomerDocumentUniqueFalse()
        {
            var customer = _fixture.GetValidCustomer();
            var newCustomer = _fixture.GetValidCustomer();

            _customerRepositoryMock.Setup(s => s.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(customer));

            var documentService = new Service.DocumentService(_customerRepositoryMock.Object, _employeeRepositoryMock.Object);
            var output = await documentService.IsDocumentUnique(newCustomer, CancellationToken.None);

            output.Should().BeFalse();

            _customerRepositoryMock.Verify(x => x.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }








        [Fact(DisplayName = "IsDocumentUnique() should return true when employee document does not exist")]
        [Trait("Domain", "Services / DocumentService")]
        public async Task IsEmployeeDocumentUnique()
        {
            var employee = _fixture.GetValidEmployee();

            _employeeRepositoryMock.Setup(s => s.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new NotFoundException());

            var documentService = new Service.DocumentService(_customerRepositoryMock.Object, _employeeRepositoryMock.Object);
            var output = await documentService.IsDocumentUnique(employee, CancellationToken.None);

            output.Should().BeTrue();

            _employeeRepositoryMock.Verify(x => x.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "IsDocumentUnique() should return true when employee exist but with same id")]
        [Trait("Domain", "Services / DocumentService")]
        public async Task IEmployeeDocumentUniqueWithSameIds()
        {
            var employee = _fixture.GetValidEmployee();

            _employeeRepositoryMock.Setup(s => s.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(employee));

            var documentService = new Service.DocumentService(_customerRepositoryMock.Object, _employeeRepositoryMock.Object);
            var output = await documentService.IsDocumentUnique(employee, CancellationToken.None);

            output.Should().BeTrue();

            _employeeRepositoryMock.Verify(x => x.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "IsDocumentUnique() should return false when employee already exists")]
        [Trait("Domain", "Services / DocumentService")]
        public async Task IsEmployeeDocumentUniqueFalse()
        {
            var employee = _fixture.GetValidEmployee();
            var newEmployee = _fixture.GetValidEmployee();

            _employeeRepositoryMock.Setup(s => s.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(employee));

            var documentService = new Service.DocumentService(_customerRepositoryMock.Object, _employeeRepositoryMock.Object);
            var output = await documentService.IsDocumentUnique(newEmployee, CancellationToken.None);

            output.Should().BeFalse();

            _employeeRepositoryMock.Verify(x => x.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
