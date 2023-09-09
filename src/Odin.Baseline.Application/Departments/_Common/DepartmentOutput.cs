using Odin.Baseline.Domain.DTO;
using Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.Application.Departments.Common
{
    public class DepartmentOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }

        public CustomerData Customer { get; set; }

        public static DepartmentOutput FromDepartment(Department department)
        {
            return new DepartmentOutput
            {
                Id = department.Id,
                Name = department.Name,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt,
                CreatedBy = department.CreatedBy,
                LastUpdatedAt = department.LastUpdatedAt,
                LastUpdatedBy = department.LastUpdatedBy,

                Customer = department.CustomerData
            };
        }

        public static IEnumerable<DepartmentOutput> FromDepartment(IEnumerable<Department> departments)
            => departments.Select(FromDepartment);
    }
}
