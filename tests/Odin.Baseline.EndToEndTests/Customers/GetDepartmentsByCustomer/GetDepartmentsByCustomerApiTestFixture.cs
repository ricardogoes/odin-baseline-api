using Odin.Baseline.EndToEndTests.Customers.Common;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Customers.GetDepartmentsByCustomer
{
    [CollectionDefinition(nameof(GetDepartmentsByCustomerApiTestCollection))]
    public class GetDepartmentsByCustomerApiTestCollection : ICollectionFixture<GetDepartmentsByCustomerApiTestFixture>
    { }

    public class GetDepartmentsByCustomerApiTestFixture : CustomerBaseFixture
    {
        public GetDepartmentsByCustomerApiTestFixture()
            : base()
        { }

        public DepartmentModel GetValidDepartmentModel(Guid? customerId = null)
        {
            var department = new DepartmentModel
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId ?? Guid.NewGuid(),
                Name = GetValidName(),
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "unit.test",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "unit.test"
            };

            return department;
        }

        public List<DepartmentModel> GetValidDepartmentsModelList(List<Guid> customersIds, int length = 10)
        {
            var departments = new List<DepartmentModel>();
            customersIds.ForEach(customerId =>
            {
                departments.AddRange(Enumerable.Range(1, length).Select(_ => GetValidDepartmentModel(customerId)).ToList());
            });

            return departments;
        }
    }
}
