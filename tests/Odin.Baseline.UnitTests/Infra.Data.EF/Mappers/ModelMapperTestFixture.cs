using Bogus.Extensions.Brazil;
using Castle.Core.Resource;
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

        public CustomerModel GetValidCustomerModel(Guid? id = null)
        {
            return new CustomerModel
            (
                id: id ?? Guid.NewGuid(),
                name: Faker.Company.CompanyName(1),
                document: Faker.Company.Cnpj(),
                isActive: true,
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr(),
                createdAt: DateTime.UtcNow,
                createdBy: "unit.testing",
                lastUpdatedAt: DateTime.UtcNow,
                lastUpdatedBy: "unit.testing"
            );
        }

        public CustomerModel GetValidCustomerModelWithoutAddress()
        {
            return new CustomerModel
            (
                id: Guid.NewGuid(),
                name: Faker.Company.CompanyName(1),
                document: Faker.Company.Cnpj(),
                isActive: true,
                createdAt: DateTime.UtcNow,
                createdBy: "unit.testing",
                lastUpdatedAt: DateTime.UtcNow,
                lastUpdatedBy: "unit.testing"
            );
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
            (
                id: Guid.NewGuid(),                
                name: Faker.Company.CompanyName(1),
                isActive: true,
                createdAt: DateTime.UtcNow,
                createdBy: "unit.testing",
                lastUpdatedAt: DateTime.UtcNow,
                lastUpdatedBy: "unit.testing",
                customerId: customerId,
                customerModel: GetValidCustomerModel(customerId)
            );
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
            (
                id: Guid.NewGuid(),                
                name: Faker.Company.CompanyName(1),
                baseSalary: 1_000,
                isActive: true,
                createdAt: DateTime.UtcNow,
                createdBy: "unit.testing",
                lastUpdatedAt: DateTime.UtcNow,
                lastUpdatedBy: "unit.testing",
                customerId: customerId,
                customerModel: GetValidCustomerModel(customerId)
            );
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
            (
                id: Guid.NewGuid(),
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                document: Faker.Person.Cpf(),
                email: Faker.Person.Email,
                isActive: true,
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr(),
                createdAt: DateTime.UtcNow,
                createdBy: "unit.testing",
                lastUpdatedAt: DateTime.UtcNow,
                lastUpdatedBy: "unit.testing",
                customerId: customerId,
                departmentId: null,
                customerModel: GetValidCustomerModel(customerId)
            );
        }

        public EmployeeModel GetValidEmployeeModelWithoutAddress()
        {
            var customerId = Guid.NewGuid();
            return new EmployeeModel
            (
               id: Guid.NewGuid(),
               firstName: Faker.Person.FirstName,
               lastName: Faker.Person.LastName,
               document: Faker.Person.Cpf(),
               email: Faker.Person.Email,
               isActive: true,
               createdAt: DateTime.UtcNow,
               createdBy: "unit.testing",
               lastUpdatedAt: DateTime.UtcNow,
               lastUpdatedBy: "unit.testing",
               customerId: customerId,
               departmentId: null,
               customerModel: GetValidCustomerModel(customerId)
           );
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
            (
                employeeId: Guid.NewGuid(),
                positionId: Guid.NewGuid(),
                salary: 10_000,
                startDate: DateTime.Now,
                finishDate: DateTime.Now,
                isActual: true
            );
        }
    }
}
