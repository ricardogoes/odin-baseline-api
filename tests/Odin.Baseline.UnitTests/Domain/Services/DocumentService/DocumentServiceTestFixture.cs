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

        public string GetValidEmployeeFistName()
            => Faker.Person.FirstName;

        public string GetValidEmployeeLastName()
            => Faker.Person.LastName;

        public string GetValidEmployeeDocument()
            => Faker.Person.Cpf();

        public string GetValidEmployeeEmail()
            => Faker.Person.Email;

        public Employee GetValidEmployee()
        {
            var employee = new Employee(Guid.NewGuid(), GetValidEmployeeFistName(), GetValidEmployeeLastName(), GetValidEmployeeDocument(), GetValidEmployeeEmail(), isActive: true);
            employee.Create("unit.testing");

            return employee;
        }

        public EmployeeModel GetValidEmployeeModel(Guid? customerId = null, Guid? departmentId = null)
        {
            var employee = new EmployeeModel
            (
                id: Guid.NewGuid(),                
                firstName: GetValidEmployeeFistName(),
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail(),
                isActive: true,
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test",
                customerId: customerId ?? Guid.NewGuid(),
                departmentId: departmentId ?? Guid.NewGuid()
            );

            return employee;
        }
    }
}
