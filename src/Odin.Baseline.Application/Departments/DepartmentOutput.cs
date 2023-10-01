using Odin.Baseline.Domain.DTO;
using Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.Application.Departments
{
    public class DepartmentOutput
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public string LastUpdatedBy { get; private set; }

        public CustomerData? Customer { get; private set; }

        public DepartmentOutput(Guid id, string name, bool isActive, DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy, CustomerData? customer = null)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
            Customer = customer;
        }

        public static DepartmentOutput FromDepartment(Department department)
        {
            return new DepartmentOutput
            (
                department.Id,
                department.Name,
                department.IsActive,
                department.CreatedAt ?? default,
                department.CreatedBy ?? "",
                department.LastUpdatedAt ?? default,
                department.LastUpdatedBy ?? ""
            );
        }

        public static IEnumerable<DepartmentOutput> FromDepartment(IEnumerable<Department> departments)
            => departments.Select(FromDepartment);
    }
}
