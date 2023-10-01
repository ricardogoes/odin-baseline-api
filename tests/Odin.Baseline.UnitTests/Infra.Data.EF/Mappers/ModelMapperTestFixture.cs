using Bogus.Extensions.Brazil;
using Odin.Baseline.Domain.Entities;
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

        public DepartmentModel GetValidDepartmentModel()
        {
            var tenantId = Guid.NewGuid();
            
            var model = new DepartmentModel
            (
                id: Guid.NewGuid(),                
                name: Faker.Company.CompanyName(1),
                isActive: true,
                createdAt: DateTime.UtcNow,
                createdBy: "unit.testing",
                lastUpdatedAt: DateTime.UtcNow,
                lastUpdatedBy: "unit.testing",
                tenantId: tenantId
            );

            model.SetAuditLog("unit.testing", created: true);

            return model;
        }

        public Position GetValidPosition()
        {
            var position = new Position(Faker.Commerce.Department(), 1_000, isActive: true);            
            return position;
        }

        public PositionModel GetValidPositionModel()
        {
            var tenantId = Guid.NewGuid();
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
                tenantId: tenantId
            );
        }

        public Employee GetValidEmployee()
        {
            var employee = new Employee(Faker.Person.FirstName, Faker.Person.LastName, Faker.Person.Cpf(), Faker.Person.Email, Guid.NewGuid(), true);
            return employee;
        }

        public EmployeeModel GetValidEmployeeModel()
        {
            var tenantId = Guid.NewGuid();
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
                tenantId: tenantId,
                departmentId: null
            );
        }

        public EmployeeModel GetValidEmployeeModelWithoutAddress()
        {
            var tenantId = Guid.NewGuid();
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
               tenantId: tenantId,
               departmentId: null
           );
        }

        public EmployeePositionHistory GetValidEmployeePositionHistory()
        {
            var positionHistory = new EmployeePositionHistory(Guid.NewGuid(), 10_000, DateTime.Now, DateTime.Now, true);
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
                isActual: true,
                tenantId: Guid.NewGuid()
            );
        }
    }
}
