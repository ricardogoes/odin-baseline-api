using Bogus.Extensions.Brazil;
using Odin.Baseline.Domain.DTO;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.ValueObjects;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.UnitTests.Infra.Data.EF.Mappers
{
    [CollectionDefinition(nameof(ModelMapperTestFixtureCollection))]
    public class ModelMapperTestFixtureCollection : ICollectionFixture<ModelMapperTestFixture>
    { }

    public class ModelMapperTestFixture : BaseFixture
    {
        public ModelMapperTestFixture()
            : base() { }

        public Customer GetValidCustomer()
        {
            var customer = new Customer(Faker.Company.CompanyName(1), Faker.Company.Cnpj(), isActive: true);
            customer.Create();
            return customer;
        }

        public CustomerModel GetValidCustomerModel(Guid? id = null)
        {
            return new CustomerModel
            {
                Id = id ?? Guid.NewGuid(),
                Name = Faker.Company.CompanyName(1),
                Document = Faker.Company.Cnpj(),
                IsActive = true,
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing"
            };
        }

        public CustomerModel GetValidCustomerModelWithoutAddress()
        {
            return new CustomerModel
            {
                Id = Guid.NewGuid(),
                Name = Faker.Company.CompanyName(1),
                Document = Faker.Company.Cnpj(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing"
            };
        }

        public Address GetValidAddress()
        {
            var address = new Address(
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

        public Department GetValidDepartment()
        {
            var department = new Department(Guid.NewGuid(), Faker.Commerce.Department(), isActive: true);
            department.Create();
            return department;
        }

        public DepartmentModel GetValidDepartmentModel()
        {
            var customerId = Guid.NewGuid();
            return new DepartmentModel
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Customer = GetValidCustomerModel(customerId),
                Name = Faker.Company.CompanyName(1),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing"
            };
        }

        public Position GetValidPosition()
        {
            var position = new Position(Guid.NewGuid(), Faker.Commerce.Department(), 1_000, isActive: true);
            position.Create();
            return position;
        }

        public PositionModel GetValidPositionModel()
        {
            var customerId = Guid.NewGuid();
            return new PositionModel
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Customer = GetValidCustomerModel(customerId),
                Name = Faker.Company.CompanyName(1),
                BaseSalary = 1_000,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing"
            };
        }

        public Employee GetValidEmployee()
        {
            var employee = new Employee(Guid.NewGuid(), Faker.Person.FirstName, Faker.Person.LastName, Faker.Person.Cpf(), Faker.Person.Email, Guid.NewGuid(), true);
            employee.Create();
            return employee;
        }

        public EmployeeModel GetValidEmployeeModel()
        {
            var customerId = Guid.NewGuid();
            return new EmployeeModel
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Customer = GetValidCustomerModel(customerId),
                FirstName = Faker.Person.FirstName,
                LastName = Faker.Person.LastName,
                Document = Faker.Person.Cpf(),
                Email = Faker.Person.Email,
                IsActive = true,
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing"
            };
        }

        public EmployeeModel GetValidEmployeeModelWithoutAddress()
        {
            var customerId = Guid.NewGuid();
            return new EmployeeModel
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Customer = GetValidCustomerModel(customerId),
                FirstName = Faker.Person.FirstName,
                LastName = Faker.Person.LastName,
                Document = Faker.Person.Cpf(),
                Email = Faker.Person.Email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing"
            };
        }

        public EmployeePositionHistory GetValidEmployeePositionHistory()
        {
            var positionHistory = new EmployeePositionHistory(Guid.NewGuid(), 10_000, DateTime.Now, DateTime.Now, true);
            positionHistory.Create();
            return positionHistory;
        }

        public EmployeePositionHistoryModel GetValidEmployeePositionHistoryModel()
        {
            return new EmployeePositionHistoryModel
            {
                EmployeeId = Guid.NewGuid(),
                PositionId = Guid.NewGuid(),
                Salary = 10_000,
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now,
                IsActual = true
            };
        }
    }
}
