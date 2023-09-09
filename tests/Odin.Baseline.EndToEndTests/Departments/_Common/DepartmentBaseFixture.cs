using Bogus;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Departments.Common
{
    public class DepartmentBaseFixture : BaseFixture
    {
        

        public List<Department> GetValidDepartmentsList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidDepartment()).ToList();

        public List<DepartmentModel> GetValidDepartmentsModelList(Guid? customerId = null, int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidDepartmentModel(customerId)).ToList();
    }
}
