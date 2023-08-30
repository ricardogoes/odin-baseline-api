using Bogus;
using Bogus.Extensions.Brazil;
using Odin.Baseline.EndToEndTests.Customers.Common;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Customers.GetEmployeesByCustomer
{
    [CollectionDefinition(nameof(GetEmployeesByCustomerApiTestCollection))]
    public class GetEmployeesByCustomerApiTestCollection : ICollectionFixture<GetEmployeesByCustomerApiTestFixture>
    { }

    public class GetEmployeesByCustomerApiTestFixture : CustomerBaseFixture
    {
        public GetEmployeesByCustomerApiTestFixture()
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

        public EmployeeModel GetValidEmployeeModel(Guid? customerId = null, Guid? departmentId = null)
        {
            var employee = new EmployeeModel
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId ?? Guid.NewGuid(),
                DepartmentId = departmentId ?? Guid.NewGuid(),
                FirstName = GetValidEmployeeFistName(),
                LastName = GetValidEmployeeLastName(),
                Document = GetValidEmployeeDocument(),
                Email = GetValidEmployeeEmail(),
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "unit.test",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "unit.test"
            };

            return employee;
        }

        public List<EmployeeModel> GetValidEmployeesModelList(List<Guid> customersIds, List<Guid>? departmentsIds = null, int length = 10)
        {
            var employees = new List<EmployeeModel>();
            foreach (var customerId in customersIds)
            {
                if (departmentsIds is not null)
                    departmentsIds.ForEach(departmentId =>
                    {
                        employees.AddRange(Enumerable.Range(1, length).Select(_ => GetValidEmployeeModel(customerId, departmentId)).ToList());
                    });
                else
                    //employees.AddRange(Enumerable.Range(1, length).Select(_ => GetValidEmployeeModel(customerId)).ToList());
                    foreach (var index in Enumerable.Range(1, length))
                    {
                        Faker = new Faker("pt_BR");
                        employees.Add(GetValidEmployeeModel(customerId));
                    }
            };

            return employees;
        }
    }
}
