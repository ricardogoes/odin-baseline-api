using Bogus.Extensions.Brazil;
using Moq;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.DomainServices;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.UnitTests.Application.Employees
{
    public abstract class EmployeeBaseFixture : BaseFixture
    {
        public Mock<IDocumentService> GetDocumentServiceMock()
            => new();

        public Mock<IEmployeeRepository> GetRepositoryMock()
            => new();

        public Mock<IUnitOfWork> GetUnitOfWorkMock()
            => new();

        public string GetValidFirstName()
            => Faker.Person.FirstName;

        public string GetValidLastName()
            => Faker.Person.LastName;

        public string GetValidDocument()
            => Faker.Person.Cpf();

        public string GetValidEmail()
            => Faker.Person.Email;

        public static string GetInvalidDocument()
            => "12.123.123/0002-12";

        public Employee GetValidEmployee(List<EmployeePositionHistory>? positionsHistory = null)
        {
            var employee = new Employee(Guid.NewGuid(), GetValidFirstName(), GetValidLastName(), GetValidDocument(), GetValidEmail(), departmentId: Guid.NewGuid(), isActive: true);

            if (positionsHistory is not null)
                positionsHistory.ForEach(employee.LoadHistoricPosition);

            return employee;
        }

        public List<Employee> GetValidEmployeesList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidEmployee()).ToList();
    }
}
