using Bogus;
using Bogus.Extensions.Brazil;
using Odin.Baseline.EndToEndTests.Controllers.Departments;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Controllers.Departments.GetEmployeesByDepartment
{
    [CollectionDefinition(nameof(GetEmployeesByDepartmentApiTestCollection))]
    public class GetEmployeesByDepartmentApiTestCollection : ICollectionFixture<GetEmployeesByDepartmentApiTestFixture>
    { }

    public class GetEmployeesByDepartmentApiTestFixture : DepartmentBaseFixture
    {
        public GetEmployeesByDepartmentApiTestFixture()
            : base()
        { }

        public string GetValidEmployeeFirstName()
           => Faker.Person.FirstName;

        public string GetValidEmployeeLastName()
            => Faker.Person.LastName;

        public string GetValidEmployeeDocument()
            => Faker.Person.Cpf();

        public string GetValidEmployeeEmail()
            => Faker.Person.Email;

        public EmployeeModel GetValidEmployeeModel(Guid? departmentId = null)
        {
            return new EmployeeModel
            (
                id: Guid.NewGuid(),
                departmentId: departmentId,
                firstName: GetValidEmployeeFirstName(),
                lastName: GetValidEmployeeLastName(),
                document: GetValidEmployeeDocument(),
                email: GetValidEmployeeEmail(),
                isActive: true,
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test",
                tenantId: TenantSinapseId
            );
        }

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
    }
}
