using Bogus.Extensions.Brazil;
using DomainEntity = Odin.Baseline.Domain.Entities;
using ValueObject = Odin.Baseline.Domain.ValueObjects;

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

        public DomainEntity.Employee GetValidEmployee(List<DomainEntity.EmployeePositionHistory> historicPositions = null)
        {
            var employee = new DomainEntity.Employee(Guid.NewGuid(), GetValidEmployeeFistName(), GetValidEmployeeLastName(), GetValidEmployeeDocument(), GetValidEmployeeEmail(), Guid.NewGuid());
            employee.Create();

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

        public ValueObject.Address GetValidAddress()
        {
            var address = new ValueObject.Address(
                Faker.Address.StreetName(),
                int.Parse(Faker.Address.BuildingNumber()),
                Faker.Address.SecondaryAddress(),
                Faker.Address.CardinalDirection(),
                Faker.Address.ZipCode(),
                Faker.Address.City(),
                Faker.Address.StateAbbr()
            );
            return address;
        }
    }
}
