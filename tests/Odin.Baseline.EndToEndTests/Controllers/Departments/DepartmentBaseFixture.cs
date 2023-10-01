using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Controllers.Departments
{
    public class DepartmentBaseFixture : BaseFixture
    {
        public List<Department> GetValidDepartmentsList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidDepartment()).ToList();

        public List<DepartmentModel> GetValidDepartmentsModelList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidDepartmentModel()).ToList();
    }
}
