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
