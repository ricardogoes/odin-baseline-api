using Bogus;
using Bogus.Extensions.Brazil;
using Moq;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.UnitTests.Domain.Services.DocumentService
{
    [CollectionDefinition(nameof(DocumentServiceTestFixtureCollection))]
    public class DocumentServiceTestFixtureCollection : ICollectionFixture<DocumentServiceTestFixture>
    { }

    public class DocumentServiceTestFixture : BaseFixture
    {
        public Mock<ICustomerRepository> GetCustomerRepository()
           => new();

        public Mock<IEmployeeRepository> GetEmployeeRepository()
          => new();

        public string GetValidCustomerName()
           => Faker.Company.CompanyName(1);

        public string GetValidCustomerDocument()
            => Faker.Company.Cnpj();

        public string GetValidEmployeeFistName()
            => Faker.Person.FirstName;

        public string GetValidEmployeeLastName()
            => Faker.Person.LastName;

        public string GetValidEmployeeDocument()
            => Faker.Person.Cpf();

        public string GetValidEmployeeEmail()
            => Faker.Person.Email;

        public Customer GetValidCustomer()
        {
            var customer = new Customer(GetValidCustomerName(), GetValidCustomerDocument(), isActive: true);
            customer.Create("unit.testing");

            return customer;
        }

        public CustomerModel GetValidCustomerModel()
        {
            var customer = new CustomerModel
            {
                Id = Guid.NewGuid(),
                Name = GetValidCustomerName(),
                Document = GetValidCustomerDocument(),
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "unit.test",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "unit.test"
            };

            return customer;
        }

        public Employee GetValidEmployee()
        {
            var employee = new Employee(Guid.NewGuid(), GetValidEmployeeFistName(), GetValidEmployeeLastName(), GetValidEmployeeDocument(), GetValidEmployeeEmail(), isActive: true);
            employee.Create("unit.testing");

            return employee;
        }

        public EmployeeModel GetValidEmployeeModel(Guid? customerId = null, Guid? departmentId = null)
        {
            var employee = new EmployeeModel
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId ?? Guid.NewGuid(),
                DepartmentId = departmentId ?? Guid.NewGuid(),
                FirstName = GetValidEmployeeFistName(),
                LastName = GetValidEmployeeLastName(),
                Document = GetValidEmployeeDocument(),
                Email = GetValidEmployeeEmail(),
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "unit.test",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "unit.test"
            };

            return employee;
        }
    }
}
