using Bogus;
using Bogus.Extensions.Brazil;
using Odin.Baseline.Infra.Data.EF.Models;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Infra.Data.EF.Repositories.Employee
{
    [CollectionDefinition(nameof(EmployeeRepositoryTestFixtureCollection))]
    public class EmployeeRepositoryTestFixtureCollection : ICollectionFixture<EmployeeRepositoryTestFixture>
    { }

    public class EmployeeRepositoryTestFixture : BaseFixture
    {
        public EmployeeRepositoryTestFixture()
            : base()
        { }

        public string GetValidEmployeeFistName()
            => Faker.Person.FirstName;

        public string GetValidEmployeeLastName()
            => Faker.Person.LastName;

        public string GetValidEmployeeDocument()
            => Faker.Person.Cpf();

        public string GetValidEmployeeEmail()
            => Faker.Person.Email;

        public DomainEntity.Employee GetValidEmployee(Guid? customerId = null)
        {
            var employee = new DomainEntity.Employee(customerId ?? Guid.NewGuid(), GetValidEmployeeFistName(), GetValidEmployeeLastName(), GetValidEmployeeDocument(), GetValidEmployeeEmail(), isActive: true);
            employee.Create("unit.testing");

            return employee;
        }

        public EmployeeModel GetValidEmployeeModel(Guid? customerId = null, Guid? departmentId = null)
        {
            return new EmployeeModel
            (
                id: Guid.NewGuid(),
                customerId: customerId ?? Guid.NewGuid(),
                departmentId: departmentId,
                firstName: GetValidEmployeeFistName(),
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail(),
                isActive: true,
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test"
            );
        }

        public List<DomainEntity.Employee> GetValidEmployeesList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidEmployee()).ToList();

        public List<EmployeeModel> GetValidEmployeesModelList(List<Guid> customersIds, List<Guid>? departmentsIds = null, int length = 10)
        {
            var employees = new List<EmployeeModel>();
            foreach(var customerId in customersIds)
            {
                if (departmentsIds is not null)
                    departmentsIds.ForEach(departmentId =>
                    {
                        employees.AddRange(Enumerable.Range(1, length).Select(_ => GetValidEmployeeModel(customerId, departmentId)).ToList());
                    });
                else
                    //employees.AddRange(Enumerable.Range(1, length).Select(_ => GetValidEmployeeModel(customerId)).ToList());
                    foreach(var index in Enumerable.Range(1, length))
                    {
                        Faker = new Faker("pt_BR");
                        employees.Add(GetValidEmployeeModel(customerId));
                    }
            };

            return employees;
        }

        public List<EmployeePositionHistoryModel> GetValidEmployeesPositionsHistoryModelList(Guid employeeId)
        {
            var positionsHistory = new List<EmployeePositionHistoryModel>
            {
                new EmployeePositionHistoryModel(employeeId,Guid.NewGuid(), 1_000, DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(-2), false),
                new EmployeePositionHistoryModel(employeeId,Guid.NewGuid(), 2_000, DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(-1), false),
                new EmployeePositionHistoryModel(employeeId,Guid.NewGuid(), 3_000, DateTime.Now.AddMonths(-1), null, true)
            };

            return positionsHistory;
        }
    }
}
