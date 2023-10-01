using Bogus;
using Bogus.Extensions.Brazil;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Controllers.Employees
{
    public class EmployeeBaseFixture : BaseFixture
    {
        public string GetValidEmployeeFirstName()
            => Faker.Person.FirstName;

        public string GetValidEmployeeLastName()
            => Faker.Person.LastName;

        public string GetValidEmployeeDocument()
            => Faker.Company.Cnpj();

        public string GetValidEmployeeEmail()
            => Faker.Person.Email;

        public Employee GetValidEmployee()
        {
            var employee = new Employee(GetValidEmployeeFirstName(), GetValidEmployeeLastName(), GetValidEmployeeDocument(), GetValidEmployeeEmail(), isActive: true);

            return employee;
        }

        public EmployeeModel GetValidEmployeeModel(Guid? departmentId = null)
        {
            return new EmployeeModel
            (
                id: Guid.NewGuid(),
                departmentId: departmentId ?? Guid.NewGuid(),
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail(),
                isActive: GetRandomBoolean(),
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test",
                tenantId: TenantSinapseId
            );
        }

        public List<Employee> GetValidEmployeesList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidEmployee()).ToList();

        public List<EmployeeModel> GetValidEmployeesModelList(List<Guid>? departmentsIds = null, int length = 10)
        {
            var employees = new List<EmployeeModel>();

            if (departmentsIds is not null)
                departmentsIds.ForEach(departmentId =>
                {
                    Faker = new Faker("pt_BR");
                    employees.AddRange(Enumerable.Range(1, length).Select(_ => GetValidEmployeeModel(departmentId)).ToList());
                });
            else
                foreach (var index in Enumerable.Range(1, length))
                {
                    Faker = new Faker("pt_BR");
                    employees.Add(GetValidEmployeeModel());
                }

            return employees;
        }

        public List<EmployeePositionHistoryModel> GetValidEmployeesPositionsHistoryModelList(Guid employeeId)
        {
            var positionsHistory = new List<EmployeePositionHistoryModel>
            {
                new EmployeePositionHistoryModel(employeeId, Guid.NewGuid(), 1_000,  DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(-2), false, TenantSinapseId),
                new EmployeePositionHistoryModel(employeeId, Guid.NewGuid(), 2_000,  DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(-1), false, TenantSinapseId),
                new EmployeePositionHistoryModel(employeeId, Guid.NewGuid(), 3_000,  DateTime.Now.AddMonths(-1), null, true, TenantSinapseId),
            };

            return positionsHistory;
        }
    }
}
