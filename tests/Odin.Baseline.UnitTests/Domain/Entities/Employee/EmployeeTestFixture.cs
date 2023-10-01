using Bogus.Extensions.Brazil;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Domain.Entities.Employee
{
    [CollectionDefinition(nameof(EmployeeTestFixtureCollection))]
    public class EmployeeTestFixtureCollection : ICollectionFixture<EmployeeTestFixture>
    { }

    public class EmployeeTestFixture : BaseFixture
    {
        public EmployeeTestFixture()
            : base() { }

        public string GetValidEmployeeFistName()
            => Faker.Person.FirstName;

        public string GetValidEmployeeLastName()
            => Faker.Person.LastName;

        public string GetValidEmployeeDocument()
            => Faker.Person.Cpf();

        public string GetValidEmployeeEmail()
            => Faker.Person.Email;

        public DomainEntity.Employee GetValidEmployee(Guid? departmentId = null, List<DomainEntity.EmployeePositionHistory>? historicPositions = null)
        {
            var employee = new DomainEntity.Employee(GetValidEmployeeFistName(), GetValidEmployeeLastName(), GetValidEmployeeDocument(), GetValidEmployeeEmail(), departmentId);
            
            if (historicPositions is not null)
            {
                foreach (var historic in historicPositions)
                {
                    employee.AddHistoricPosition(historic);
                }
            }

            return employee;
        }

        public DomainEntity.EmployeePositionHistory GetValidHistoricPosition()
        {
            return new DomainEntity.EmployeePositionHistory(Guid.NewGuid(), 10_000, DateTime.UtcNow, null, true);
        }        
    }
}
